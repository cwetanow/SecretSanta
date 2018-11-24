using SecretSanta.Models;

namespace SecretSanta.Factories
{
    public interface IGroupUserFactory
    {
        GroupUser CreateGroupUser(int groupId, string userId);
    }
}
