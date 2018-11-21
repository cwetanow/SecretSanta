using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Models
{
    public class Gift
    {
        public Gift()
        {

        }

        public Gift(int groupId, string senderId, string receiverId)
        {
            this.GroupId = groupId;
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
        }

        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        [Required]
        public string SenderId { get; set; }

        public User Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public User Receiver { get; set; }
    }
}
