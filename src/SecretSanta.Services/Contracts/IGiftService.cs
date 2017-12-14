using SecretSanta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Services.Contracts
{
    public interface IGiftService
    {
        Gift GetGiftInGroup(int groupId, string senderId);

        Task<Gift> CreateGiftAsync(int groupId, User sender, User receiver);

        Task<IEnumerable<Gift>> DistributeGifts(IEnumerable<User> groupUsers);
    }
}
