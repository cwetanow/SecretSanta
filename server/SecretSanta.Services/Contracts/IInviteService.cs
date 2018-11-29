using SecretSanta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Services.Contracts
{
    public interface IInviteService
    {
        IEnumerable<Invite> GetPendingInvites(string userId, bool orderByAscending, int limit, int offset);

        Task<Invite> CreateInviteAsync(int groupId, string userId);

        bool IsUserInvited(int groupId, string userId);

        Task RemoveInvite(int groupId, string userId);
    }
}
