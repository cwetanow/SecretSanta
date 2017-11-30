using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Web.Models.Account;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Factories;

namespace SecretSanta.Web.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAuthenticationProvider authenticationProvider;
        private readonly IUserFactory userFactory;

        public AccountController(IAuthenticationProvider authenticationProvider,
            IUserFactory userFactory)
        {
            this.authenticationProvider = authenticationProvider;
            this.userFactory = userFactory;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var user = await this.authenticationProvider.FindByUsernameAsync(model.Username);

            if (user == null)
            {
                user = this.userFactory.CreateUser(model.Username, model.Email, model.DisplayName);

                var result = await this.authenticationProvider.RegisterUser(user, model.Password);

                if (result == IdentityResult.Success)
                {
                    return this.Ok(user);
                }
            }

            return this.BadRequest();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        //{
        //    var user = await this.authenticationProvider.FindByEmailAsync(model.Email);

        //    if (user != null)
        //    {
        //        var result = await this.authenticationProvider.CheckPasswordSignInAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            // Generate token
        //        }
        //    }

        //    return this.BadRequest();
        //}
    }
}