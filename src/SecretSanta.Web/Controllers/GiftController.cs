using Microsoft.AspNetCore.Mvc;
using SecretSanta.Authentication.Contracts;
using SecretSanta.Services.Contracts;
using SecretSanta.Web.Infrastructure;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    [Route("api/gifts")]
    public class GiftController : Controller
    {
        private readonly IGiftService service;
        private readonly IAuthenticationProvider authenticationProvider;
        private readonly IDtoFactory factory;
        private readonly IGroupService groupService;
        private readonly IInviteService inviteService;

        public GiftController(IGiftService service,
            IAuthenticationProvider authenticationProvider,
            IDtoFactory factory,
            IGroupService groupService,
            IInviteService inviteService)
        {
            this.service = service;
            this.authenticationProvider = authenticationProvider;
            this.factory = factory;
            this.groupService = groupService;
            this.inviteService = inviteService;
        }

        [HttpGet]
        [Route("{groupName}")]
        public async Task<IActionResult> GetGroupGift(string groupName)
        {
            var user = await this.authenticationProvider.GetCurrentUserAsync();

            var group = this.groupService.GetByName(groupName);

            if (group == null)
            {
                return this.NotFound();
            }

            var isInvited = this.inviteService.IsUserInvited(group.Id, user.Id);

            if (!isInvited)
            {   
                return this.Forbid();
            }

            var gift = this.service.GetGiftInGroup(group.Id, user.Id);

            var hasGift = gift != null;

            var giftDto = this.factory.CreateGiftDto(gift);

            var dto = this.factory.CreateHasGiftDto(hasGift, giftDto);

            return this.Ok(dto);
        }

        [HttpPost]
        [Route("distribute/{groupName}")]
        public async Task<IActionResult> DistributeGifts(string groupName)
        {
            var user = await this.authenticationProvider.GetCurrentUserAsync();

            var group = this.groupService.GetByName(groupName);

            if (group == null)
            {
                return this.NotFound();
            }

            if (user.Id != group.OwnerId)
            {
                return this.Forbid();
            }

            var gifts = this.service.DistributeGifts(group);

            return this.Ok(gifts);
        }
    }
}
