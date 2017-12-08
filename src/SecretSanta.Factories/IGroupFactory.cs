using SecretSanta.Models;

namespace SecretSanta.Factories
{
    public interface IGroupFactory
    {
        Group CreateGroup(string groupName, string ownerId);
    }
}
