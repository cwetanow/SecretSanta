using System;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Web.Models.Account;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SecretSanta.Common;
using SecretSanta.Factories;
using SecretSanta.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace SecretSanta.Web.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAuthenticationProvider authenticationProvider;
        private readonly IUserFactory userFactory;
        private readonly IDtoFactory dtoFactory;

        public AccountController(IAuthenticationProvider authenticationProvider,
            IUserFactory userFactory,
            IDtoFactory dtoFactory)
        {
            if (authenticationProvider == null)
            {
                throw new ArgumentNullException(nameof(authenticationProvider));
            }

            if (userFactory == null)
            {
                throw new ArgumentNullException(nameof(userFactory));
            }

            if (dtoFactory == null)
            {
                throw new ArgumentNullException(nameof(dtoFactory));
            }

            this.authenticationProvider = authenticationProvider;
            this.userFactory = userFactory;
            this.dtoFactory = dtoFactory;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
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
        [AllowAnonymous]
        [Route("token")]
        public async Task<IActionResult> GenerateToken([FromBody]LoginDto model)
        {
            var user = await this.authenticationProvider.FindByUsernameAsync(model.Username);

            if (user != null)
            {
                var result = await this.authenticationProvider.CheckPasswordSignInAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = this.authenticationProvider.GenerateToken(user.Email);

                    var dto = this.dtoFactory.CreateTokenDto(token);

                    return this.Ok(dto);
                }
            }

            return this.BadRequest(Constants.InvalidCredentials);
        }
    }
}