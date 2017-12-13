using System.Threading.Tasks;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using System.Linq;

namespace SecretSanta.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IRepository<GroupUser> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IGroupUserFactory factory;

        public MembershipService(IRepository<GroupUser> repository, IUnitOfWork unitOfWork, IGroupUserFactory factory)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
        }

        public async Task<GroupUser> JoinGroup(int groupId, string userId)
        {
            var groupUser = this.repository.All
                .FirstOrDefault(g => g.UserId.Equals(userId) && g.GroupId.Equals(groupId));

            if (groupUser != null)
            {
                return null;
            }

            groupUser = this.factory.CreateGroupUser(groupId, userId);

            this.repository.Add(groupUser);
            await this.unitOfWork.CommitAsync();

            return groupUser;
        }
    }
}
