using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;

namespace SecretSanta.Authentication
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenManager tokenManager;

        public AuthenticationProvider(UserManager<User> userManager, SignInManager<User> signInManager, ITokenManager tokenManager)
        {
            if (userManager == null)
            {
                throw new System.ArgumentNullException(nameof(userManager));
            }

            if (signInManager == null)
            {
                throw new System.ArgumentNullException(nameof(signInManager));
            }

            if (tokenManager == null)
            {
                throw new System.ArgumentNullException(nameof(tokenManager));
            }

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenManager = tokenManager;
        }

        public Task<User> FindByUsernameAsync(string username)
        {
            return this.userManager.FindByNameAsync(username);
        }

        public Task<IdentityResult> RegisterUser(User user, string password)
        {
            return this.userManager.CreateAsync(user, password);
        }

        public Task<SignInResult> CheckPasswordSignInAsync(User user, string password)
        {
            return this.signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public string GenerateToken(string email)
        {
            var token = this.tokenManager.GenerateToken(email);

            return token;
        }
    }
}
