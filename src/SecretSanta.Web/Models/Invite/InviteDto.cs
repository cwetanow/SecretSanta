using System;
using SecretSanta.Models.Enums;
using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Models.Invite
{
    public class InviteDto
    {
        public InviteDto()
        {

        }

        public InviteDto(InviteState state, DateTime date)
        {
            this.State = state;
            this.Date = date;
        }

        public InviteState State { get; set; }

        public DateTime Date { get; set; }

        public static Func<SecretSanta.Models.Invite, InviteDto> FromInvite
        {
            get { return (invite) => new InviteDto(invite.State, invite.Date); }
        }
    }
}
