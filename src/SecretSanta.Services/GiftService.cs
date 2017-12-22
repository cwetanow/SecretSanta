using Microsoft.EntityFrameworkCore;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SecretSanta.Providers.Contracts;

namespace SecretSanta.Services
{
    public class GiftService : IGiftService
    {
        private readonly IRepository<Gift> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGiftFactory factory;
        private readonly IGiftManager giftManager;
        private readonly IGroupService groupService;

        public GiftService(IRepository<Gift> repository, IUnitOfWork unitOfWork, IGiftFactory factory, IGiftManager giftManager,
            IGroupService groupService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
            this.giftManager = giftManager;
            this.groupService = groupService;
        }

        public Gift GetGiftInGroup(int groupId, string senderId)
        {
            var gift = this.repository.All
                .Include(g => g.Group)
                .Include(g => g.Sender)
                .Include(g => g.Receiver)
                .FirstOrDefault(g => g.GroupId.Equals(groupId) && g.SenderId.Equals(senderId));

            return gift;
        }

        public async Task<Gift> CreateGiftAsync(int groupId, User sender, User receiver)
        {
            var gift = this.factory.CreateGift(groupId, sender.Id, receiver.Id);

            this.repository.Add(gift);

            await this.unitOfWork.CommitAsync();

            sender.SentGifts.Add(gift);
            receiver.ReceivedGifts.Add(gift);

            await this.unitOfWork.CommitAsync();

            return gift;
        }

        public async Task<IEnumerable<Gift>> DistributeGifts(Group group)
        {
            var groupUsers = this.groupService.GetGroupUsers(group.GroupName)
                .ToList();

            var gifts = this.giftManager.DistributeGifts(groupUsers, group.Id)
                .ToList();

            foreach (var gift in gifts)
            {
                this.repository.Add(gift);
            }

            await this.unitOfWork.CommitAsync();

            return gifts;
        }
    }
}
