using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Models;
using Microsoft.AspNetCore.Http;

namespace SecretSanta.Authentication
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly UserManager<User> userManager;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly ITokenManager tokenManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationProvider(UserManager<User> userManager,
            IPasswordHasher<User> passwordHasher,
            ITokenManager tokenManager,
            IHttpContextAccessor httpContextAccessor)
        {
            if (userManager == null)
            {
                throw new System.ArgumentNullException(nameof(userManager));
            }

            if (passwordHasher == null)
            {
                throw new System.ArgumentNullException(nameof(passwordHasher));
            }

            if (tokenManager == null)
            {
                throw new System.ArgumentNullException(nameof(tokenManager));
            }

            if (httpContextAccessor == null)
            {
                throw new System.ArgumentNullException(nameof(httpContextAccessor));
            }

            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.tokenManager = tokenManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<User> FindByUsernameAsync(string username)
        {
            return this.userManager.FindByNameAsync(username);
        }

        public Task<IdentityResult> RegisterUser(User user, string password)
        {
            return this.userManager.CreateAsync(user, password);
        }

        public PasswordVerificationResult CheckPasswordSignIn(User user, string password)
        {
            var result = this.passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result;
        }

        public string GenerateToken(string email)
        {
            var token = this.tokenManager.GenerateToken(email);

            return token;
        }

        public Task<User> GetCurrentUserAsync()
        {
            var principal = this.httpContextAccessor.HttpContext.User;

            return this.userManager.GetUserAsync(principal);
        }
    }
}
