using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Models.Membership;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
	public class MembershipController : Controller
	{
		private readonly IMembershipService membershipService;
		private readonly IInviteService inviteService;
		private readonly IGroupService groupService;
		private readonly IAuthenticationProvider authenticationProvider;

		public MembershipController(IMembershipService membershipService,
			IInviteService inviteService,
			IGroupService groupService,
			IAuthenticationProvider authenticationProvider)
		{
			this.membershipService = membershipService;
			this.inviteService = inviteService;
			this.groupService = groupService;
			this.authenticationProvider = authenticationProvider;
		}

		[HttpPost]
		[Route("api/groups/members")]
		public async Task<IActionResult> AnswerInvite([FromBody]AnswerInviteDto dto)
		{
			var userTask = this.authenticationProvider.GetCurrentUserAsync();

			var group = this.groupService.GetByName(dto.GroupName);

			if (group == null)
			{
				return this.NotFound();
			}

			var user = await userTask;

			if (user == null)
			{
				return this.NotFound();
			}

			var isInvited = this.inviteService.IsUserInvited(group.Id, user.Id);

			if (!isInvited)
			{
				return this.Forbid();
			}

			if (dto.Accepted)
			{
				await this.membershipService.JoinGroup(group.Id, user.Id);
			}

			await this.inviteService.RemoveInvite(group.Id, user.Id);

			return this.NoContent();
		}
	}
}
