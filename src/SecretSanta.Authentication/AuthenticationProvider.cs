using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;

namespace SecretSanta.Authentication
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly UserManager<User> userManager;

        public AuthenticationProvider(UserManager<User> userManager)
        {
            if (userManager == null)
            {
                throw new System.ArgumentNullException(nameof(userManager));
            }

            this.userManager = userManager;
        }

        public Task<User> FindByUsernameAsync(string username)
        {
            return this.userManager.FindByNameAsync(username);
        }

        public Task<IdentityResult> RegisterUser(User user, string password)
        {
            return this.userManager.CreateAsync(user, password);
        }
    }
}
