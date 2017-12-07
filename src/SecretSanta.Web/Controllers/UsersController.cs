using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Common;

namespace SecretSanta.Web.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IDtoFactory dtoFactory;

        public UsersController(IUserService userService, IDtoFactory dtoFactory)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            if (dtoFactory == null)
            {
                throw new ArgumentNullException(nameof(dtoFactory));
            }

            this.userService = userService;
            this.dtoFactory = dtoFactory;
        }

        public IActionResult Get([FromQuery]int offset = 0, [FromQuery]int limit = 10,
            [FromQuery]bool sortAscending = true, [FromQuery]string searchPattern = null)
        {
            var result = this.userService.GetUsers(offset, limit, sortAscending, searchPattern);

            var dto = this.dtoFactory.CreateUsersListDto(result);

            return this.Ok(dto);
        }

        [Route("{username}")]
        public IActionResult GetByUsername([FromQuery]string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return this.BadRequest(Constants.UsernameCannotBeNull);
            }

            var user = this.userService.GetByUsername(username);

            if (user == null)
            {
                return this.NotFound(Constants.UserNotFound);
            }

            var dto = this.dtoFactory.CreateUserDto(user.UserName, user.Email, user.DisplayName);

            return this.Ok(dto);
        }
    }
}
