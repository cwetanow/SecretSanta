using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Web.Models.Invite
{
    public class InviteListDto
    {
        public InviteListDto()
        {
            this.Invites = new List<InviteDto>();
        }

        public InviteListDto(IEnumerable<SecretSanta.Models.Invite> invites)
            : this()
        {
            this.Invites = invites
                .Select(InviteDto.FromInvite)
                .ToList();
        }

        public IEnumerable<InviteDto> Invites { get; set; }
    }
}
