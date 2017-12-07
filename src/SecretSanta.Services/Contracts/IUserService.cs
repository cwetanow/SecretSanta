using System.Collections.Generic;
using SecretSanta.Models;

namespace SecretSanta.Services.Contracts
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers(int offset, int limit, bool sortAscending = true, string searchPattern = null);
    }
}
