using SecretSanta.Models;
using System.Threading.Tasks;

namespace SecretSanta.Services.Contracts
{
    public interface IMembershipService
    {
        Task<GroupUser> JoinGroup(int groupId, string userId);
    }
}
