using System;
using SecretSanta.Models.Enums;

namespace SecretSanta.Web.Models.Invite
{
    public class InviteDto
    {
        public InviteDto()
        {

        }
        
        public InviteDto(SecretSanta.Models.Invite invite)
        {
            this.State = invite.State;
            this.Date = invite.Date;
            this.GroupName = invite.Group.GroupName;
        }

        public InviteState State { get; set; }

        public DateTime Date { get; set; }

        public string GroupName { get; set; }

        public static Func<SecretSanta.Models.Invite, InviteDto> FromInvite
        {
            get { return (invite) => new InviteDto(invite); }
        }
    }
}
