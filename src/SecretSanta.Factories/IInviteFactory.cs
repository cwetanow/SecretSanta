using SecretSanta.Models;

namespace SecretSanta.Factories
{
    public interface IInviteFactory
    {
        Invite CreateInvite(int groupId, string userId);
    }
}
