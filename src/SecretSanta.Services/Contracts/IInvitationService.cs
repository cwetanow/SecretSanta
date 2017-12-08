using SecretSanta.Models;
using System.Collections.Generic;

namespace SecretSanta.Services.Contracts
{
    public interface IInviteService
    {
        IEnumerable<Invite> GetPendingInvites(string userId);
    }
}
