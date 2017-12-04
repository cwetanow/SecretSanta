using System;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Web.Models.Account;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Common;
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
            if (authenticationProvider == null)
            {
                throw new ArgumentNullException(nameof(authenticationProvider));
            }

            if (userFactory == null)
            {
                throw new ArgumentNullException(nameof(userFactory));
            }

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

                return this.BadRequest();
            }

            return this.BadRequest(Constants.UserAlreadyExists);
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> GenerateToken([FromBody]LoginViewModel model)
        {
            var user = await this.authenticationProvider.FindByUsernameAsync(model.Username);

            if (user != null)
            {
                var result = await this.authenticationProvider.CheckPasswordSignInAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = this.authenticationProvider.GenerateToken(user.Email);

                    return this.Ok(new {token});
                }
            }

            return this.BadRequest();
        }
    }
}