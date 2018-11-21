using SecretSanta.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Models
{
    public class Invite
    {
        public Invite()
        {
            this.State = InviteState.Pending;
        }

        public Invite(int groupId, string userId, DateTime date)
            : this()
        {
            this.GroupId = groupId;
            this.UserId = userId;
            this.Date = date;
        }

        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public InviteState State { get; set; }

        public DateTime Date { get; set; }
    }
}
