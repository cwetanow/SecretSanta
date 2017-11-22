using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SecretSanta.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }

        public ICollection<Invite> Invites { get; set; }

        public ICollection<Group> Groups { get; set; }

        public ICollection<GroupUser> JoinedGroups { get; set; }

        public ICollection<Gift> SentGifts { get; set; }

        public ICollection<Gift> ReceivedGifts { get; set; }
    }
}
