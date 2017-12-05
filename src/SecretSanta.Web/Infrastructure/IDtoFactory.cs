using System.Collections.Generic;
using SecretSanta.Models;
using SecretSanta.Web.Models.Account;
using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Infrastructure
{
    public interface IDtoFactory
    {
        TokenDto CreateTokenDto(string token);

        UsersListDto CreateUsersListDto(IEnumerable<User> users);
    }
}
