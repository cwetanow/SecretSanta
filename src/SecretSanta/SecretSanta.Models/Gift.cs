namespace SecretSanta.Models
{
    public class Gift
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }

        public string SenderId { get; set; }

        public User Sender { get; set; }

        public string ReceiverId { get; set; }

        public User Receiver { get; set; }
    }
}
