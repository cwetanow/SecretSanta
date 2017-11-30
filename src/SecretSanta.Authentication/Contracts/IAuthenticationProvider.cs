using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SecretSanta.Models;

namespace SecretSanta.Authentication.Contracts
{
    public interface IAuthenticationProvider
    {
        Task<User> FindByUsernameAsync(string username);

        Task<IdentityResult> RegisterUser(User user, string password);
    }
}
