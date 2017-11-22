using Microsoft.AspNetCore.Identity;
using SecretSanta.Models;
using System.Threading.Tasks;

namespace SecretSanta.Authentication.Contracts
{
    public interface IAuthenticationProvider
    {
        Task<User> FindByEmailAsync(string email);

        Task<SignInResult> CheckPasswordSignInAsync(User user, string password);

        Task<IdentityResult> RegisterUser(User user, string password);

        Task SignOutAsync();
    }
}
