using System.Collections.Generic;

namespace SecretSanta.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public ICollection<GroupUser> Users { get; set; }

        public ICollection<Invite> Invites { get; set; }
    }
}
