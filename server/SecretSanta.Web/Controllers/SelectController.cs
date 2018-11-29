using Microsoft.AspNetCore.Mvc;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Infrastructure;

namespace SecretSanta.Web.Controllers
{
	[Route("api/select")]
	public class SelectController : Controller
    {
		private readonly IUserService userService;
		private readonly IDtoFactory dtoFactory;

		public SelectController(IUserService userService, IDtoFactory dtoFactory)
		{
			this.userService = userService;
			this.dtoFactory = dtoFactory;
		}

		[HttpGet]
		[Route("inviteUsers/{groupName}")]
		public IActionResult GetUsersNotInGroup(string groupName, string searchPattern = null)
        {
			var users = this.userService.GetUsersNotInGroup(groupName, searchPattern);

			var dto = this.dtoFactory.CreateUsersListDto(users);

			return this.Ok(dto);
        }
    }
}