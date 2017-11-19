using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SecretSanta.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }

        public ICollection<Invite> Invites { get; set; }

        public ICollection<Group> Groups { get; set; }

        public ICollection<Gift> Gifts { get; set; }
    }
}
