using SecretSanta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Services.Contracts
{
    public interface IInviteService
    {
        IEnumerable<Invite> GetPendingInvites(string userId, bool orderByAscending, int limit, int offset);

        Task<bool> CreateInviteAsync(int groupId, string userId);
    }
}
