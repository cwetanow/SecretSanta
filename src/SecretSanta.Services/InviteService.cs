using System.Collections.Generic;
using SecretSanta.Models;
using SecretSanta.Services.Contracts;
using SecretSanta.Data.Contracts;
using SecretSanta.Factories;
using System.Linq;
using SecretSanta.Models.Enums;

namespace SecretSanta.Services
{
    public class InviteService : IInviteService
    {
        private readonly IRepository<Invite> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IInviteFactory factory;

        public InviteService(IRepository<Invite> repository, IUnitOfWork unitOfWork, IInviteFactory factory)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.factory = factory;
        }

        public IEnumerable<Invite> GetPendingInvites(string userId)
        {
            var invites = this.repository.All
                .Where(i => i.State == InviteState.Pending && i.UserId.Equals(userId))
                .ToList();

            return invites;
        }
    }
}
