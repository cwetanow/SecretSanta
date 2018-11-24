using SecretSanta.Models;

namespace SecretSanta.Factories
{
    public interface IGiftFactory
    {
        Gift CreateGift(int groupId, string senderId, string receiverId);
    }
}
