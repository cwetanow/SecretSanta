using SecretSanta.Models.Enums;

namespace SecretSanta.Models
{
    public class Invite
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public InviteState State { get; set; }
    }
}
