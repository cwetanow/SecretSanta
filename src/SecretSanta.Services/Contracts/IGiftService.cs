using SecretSanta.Models;
using System.Threading.Tasks;

namespace SecretSanta.Services.Contracts
{
    public interface IGiftService
    {
        Gift GetGiftInGroup(int groupId, string senderId);

        Task<Gift> CreateGift(int groupId, string senderId, string receiverId);
    }
}
