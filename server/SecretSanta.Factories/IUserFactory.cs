using SecretSanta.Models;

namespace SecretSanta.Factories
{
    public interface  IUserFactory
    {
        User CreateUser(string username, string email, string displayName);
    }
}
