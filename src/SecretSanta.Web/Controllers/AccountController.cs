using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Web.Models.Account;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAuthenticationProvider authenticationProvider;
        private readonly ITokenProvider tokenProvider;

        public AccountController(IAuthenticationProvider authenticationProvider, 
            ITokenProvider tokenProvider)
        {
            this.authenticationProvider = authenticationProvider;
            this.tokenProvider = tokenProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var user = await this.authenticationProvider.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await this.authenticationProvider.CheckPasswordSignInAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Generate token
                }
            }

            return this.BadRequest();
        }
    }
}