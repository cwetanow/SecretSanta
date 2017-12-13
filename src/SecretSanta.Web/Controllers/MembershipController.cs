using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    [Route("api/membership")]
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
        [Route("answerInvite")]
        public async Task<IActionResult> AnswerInvite([FromBody]string groupName, [FromBody]string username, [FromBody]bool accepted)
        {
            var userTask = this.authenticationProvider.FindByUsernameAsync(username);

            var group = this.groupService.GetByName(groupName);

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

            if (accepted)
            {
                await this.membershipService.JoinGroup(group.Id, user.Id);
            }
            else
            {
                await this.inviteService.CancelInvite(group.Id, user.Id);
            }

            return this.NoContent();
        }
    }
}
