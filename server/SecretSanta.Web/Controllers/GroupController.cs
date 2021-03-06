using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Web.Models.Group;
using System;
using System.Threading.Tasks;
using SecretSanta.Common;
using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Controllers
{
	[Route("api/groups")]
	public class GroupController : Controller
	{
		private readonly IGroupService groupService;
		private readonly IDtoFactory factory;
		private readonly IAuthenticationProvider authenticationProvider;
		private readonly IMembershipService membershipService;

		public GroupController(IGroupService groupService, IDtoFactory factory, IAuthenticationProvider authenticationProvider, IMembershipService membershipService)
		{
			if (groupService == null)
			{
				throw new ArgumentNullException(nameof(groupService));
			}

			if (authenticationProvider == null)
			{
				throw new ArgumentNullException(nameof(authenticationProvider));
			}

			if (factory == null)
			{
				throw new ArgumentNullException(nameof(factory));
			}

			this.groupService = groupService;
			this.factory = factory;
			this.authenticationProvider = authenticationProvider;
			this.membershipService = membershipService;
		}

		[HttpPost]
		[Route("")]
		public async Task<IActionResult> CreateGroup([FromBody]CreateGroupDto dto)
		{
			if (string.IsNullOrEmpty(dto.GroupName))
			{
				return this.BadRequest(Constants.GroupNameCannotBeNull);
			}

			var user = await this.authenticationProvider.GetCurrentUserAsync();

			var group = await this.groupService.CreateGroupAsync(dto.GroupName, user.Id);

			if (group == null)
			{
				return this.BadRequest(Constants.GroupAlreadyExists);
			}

			await this.membershipService.JoinGroup(group.Id, user.Id);

			group.Owner = user;

			var resultDto = this.factory.CreateGroupDto(group);

			return this.Ok(resultDto);
		}

		[HttpGet]
		[Route("{groupName}/users")]
		public async Task<IActionResult> GetGroupUsers(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				return this.BadRequest(Constants.GroupNameCannotBeNull);
			}

			var user = await this.authenticationProvider.GetCurrentUserAsync();

			var group = this.groupService.GetByName(groupName);

			var users = this.groupService.GetGroupUsers(groupName);

			var resultDto = this.factory.CreateUsersListDto(users);

			return this.Ok(resultDto);
		}

		[HttpGet]
		[Route("user")]
		public async Task<IActionResult> GetUserGroups()
		{
			var user = await this.authenticationProvider.GetCurrentUserAsync();

			var groups = this.groupService.GetUserGroups(user.Id);

			var dto = this.factory.CreateGroupListDto(groups);

			return this.Ok(dto);
		}

		[HttpGet]
		[Route("{groupName}/checkOwner")]
		public async Task<IActionResult> CheckGroupOwner(string groupName)
		{
			var user = await this.authenticationProvider.GetCurrentUserAsync();

			var isUserOwner = this.groupService.IsUserOwner(groupName, user.Id);

			return this.Ok(new { isUserOwner });
		}

		[HttpDelete]
		[Route("{groupName}/users")]
		public async Task<IActionResult> RemoveUserFromGroup(string groupName, [FromBody]UserDto dto)
		{
			var currentUserTask = this.authenticationProvider.GetCurrentUserAsync();

			var group = this.groupService.GetByName(groupName);

			if (group == null)
			{
				return this.NotFound();
			}

			var currentUser = await currentUserTask;

			if (!currentUser.Id.Equals(group.OwnerId))
			{
				return this.Forbid();
			}

			var user = await this.authenticationProvider.FindByUsernameAsync(dto.Username);

			if (user == null)
			{
				return this.NotFound();
			}

			await this.groupService.RemoveUserFromGroup(group.Id, user.Id);

			return this.NoContent();
		}

		[HttpPost]
		[Route("{groupName}/closed")]
		public async Task<IActionResult> CloseGroup(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				return this.BadRequest(Constants.GroupNameCannotBeNull);
			}

			var user = await this.authenticationProvider.GetCurrentUserAsync();

			var isOwner = this.groupService.IsUserOwner(groupName, user.Id);
			if (!isOwner)
			{
				return Forbid();
			}

			await this.groupService.CloseGroup(groupName);

			return this.NoContent();
		}

		[HttpGet]
		[Route("{groupName}/closed")]
		public IActionResult IsGroupClosed(string groupName)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				return this.BadRequest(Constants.GroupNameCannotBeNull);
			}

			var isGroupClosed = this.groupService.IsGroupClosed(groupName);

			if (!isGroupClosed.HasValue)
			{
				return NotFound();
			}

			return this.Ok(isGroupClosed);
		}
	}
}
