﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Infrastructure;
using SecretSanta.Common;
using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Controllers
{
	[Route("api/invites")]
	public class InviteController : Controller
	{
		private readonly IAuthenticationProvider authenticationProvider;
		private readonly IInviteService service;
		private readonly IDtoFactory dtoFactory;
		private readonly IGroupService groupService;

		public InviteController(IAuthenticationProvider authenticationProvider, IInviteService service,
			IDtoFactory dtoFactory, IGroupService groupService)
		{
			this.authenticationProvider = authenticationProvider;
			this.service = service;
			this.dtoFactory = dtoFactory;
			this.groupService = groupService;
		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> GetPendingUserInvites([FromQuery]int offset = 0, [FromQuery]int limit = 10,
			[FromQuery]bool sortAscending = true)
		{
			var user = await this.authenticationProvider.GetCurrentUserAsync();

			var invites = this.service.GetPendingInvites(user.Id, sortAscending, limit, offset);

			var dto = this.dtoFactory.CreateInviteListDto(invites);

			return this.Ok(dto);
		}

		[HttpPost]
		[Route("{groupName}")]
		public async Task<IActionResult> SendInvite(string groupName, [FromBody]UserDto userDto)
		{
			if (string.IsNullOrEmpty(groupName))
			{
				return this.BadRequest(Constants.GroupNameCannotBeNull);
			}

			if (string.IsNullOrEmpty(userDto.Username))
			{
				return this.BadRequest(Constants.UsernameCannotBeNull);
			}

			var group = this.groupService.GetByName(groupName);

			if (group == null)
			{
				return this.NotFound();
			}

			var currentUser = await this.authenticationProvider.GetCurrentUserAsync();

			if (!currentUser.Id.Equals(group.OwnerId))
			{
				return this.Forbid();
			}

			var user = await this.authenticationProvider.FindByUsernameAsync(userDto.Username);

			if (user == null)
			{
				return this.NotFound();
			}

			var invite = await this.service.CreateInviteAsync(group.Id, user.Id);

			var dto = this.dtoFactory.CreateInviteDto(invite);

			return this.Ok(dto);
		}
	}
}
