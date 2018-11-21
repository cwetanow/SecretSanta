using System.Collections.Generic;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using System.Linq;
using SecretSanta.Models.Enums;
using SecretSanta.Providers.Contracts;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SecretSanta.Services
{
    public class InviteService : IInviteService
    {
        private readonly IRepository<Invite> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IInviteFactory factory;
        private readonly IDateTimeProvider dateTimeProvider;

        public InviteService(IRepository<Invite> repository, IUnitOfWork unitOfWork, IInviteFactory factory, IDateTimeProvider dateTimeProvider)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
            this.dateTimeProvider = dateTimeProvider;
        }

        public IEnumerable<Invite> GetPendingInvites(string userId, bool orderByAscending, int limit, int offset)
        {
            var invites = this.repository.All
                .Include(i => i.Group)
                .Where(i => i.State == InviteState.Pending && i.UserId.Equals(userId));

            if (orderByAscending)
            {
                invites = invites
                    .OrderBy(i => i.Date);
            }
            else
            {
                invites = invites
                   .OrderByDescending(i => i.Date);
            }

            invites = invites
                .Skip(offset)
                .Take(limit);

            return invites.ToList();
        }

        public async Task<Invite> CreateInviteAsync(int groupId, string userId)
        {
            var date = this.dateTimeProvider.GetCurrentTime();

            var invite = this.factory.CreateInvite(groupId, userId, date);

            this.repository.Add(invite);
            await this.unitOfWork.CommitAsync();

            return invite;
        }

        public bool IsUserInvited(int groupId, string userId)
        {
            var invite = this.repository.All
                       .FirstOrDefault(i => i.GroupId.Equals(groupId) && i.UserId.Equals(userId));

            var hasInvite = invite != null;

            return hasInvite;
        }

        public async Task CancelInvite(int groupId, string userId)
        {
            var invite = this.repository.All
                .FirstOrDefault(i => i.GroupId.Equals(groupId) && i.UserId.Equals(userId));

            if (invite != null)
            {
                this.repository.Delete(invite);
                await this.unitOfWork.CommitAsync();
            }
        }
    }
}
