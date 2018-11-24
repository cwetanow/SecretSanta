using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SecretSanta.Models
{
    public class User : IdentityUser
    {
        public User()
        {

        }

        public User(string username, string email, string displayName) : this()
        {
            this.UserName = username;
            this.Email = email;
            this.DisplayName = displayName;
        }

        public string DisplayName { get; set; }

        public ICollection<Invite> Invites { get; set; }

        public ICollection<Group> Groups { get; set; }

        public ICollection<GroupUser> JoinedGroups { get; set; }

        public ICollection<Gift> SentGifts { get; set; }

        public ICollection<Gift> ReceivedGifts { get; set; }
    }
}
