using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Infrastructure;

namespace SecretSanta.Web.Controllers
{
    [Route("api/invites")]
    public class InviteController : Controller
    {
        private readonly IAuthenticationProvider authenticationProvider;
        private readonly IInviteService service;
        private readonly IDtoFactory dtoFactory;

        public InviteController(IAuthenticationProvider authenticationProvider, IInviteService service, IDtoFactory dtoFactory)
        {
            this.authenticationProvider = authenticationProvider;
            this.service = service;
            this.dtoFactory = dtoFactory;
        }

        [HttpGet]
        [Route("pending")]
        public async Task<IActionResult> GetPendingUserInvites([FromQuery]int offset = 0, [FromQuery]int limit = 10,
            [FromQuery]bool sortAscending = true)
        {
            var user = await this.authenticationProvider.GetCurrentUserAsync();

            var invites = this.service.GetPendingInvites(user.Id, sortAscending, limit, offset);

            var dto = this.dtoFactory.CreateInviteListDto(invites);

            return this.Ok(dto);
        }
    }
}
