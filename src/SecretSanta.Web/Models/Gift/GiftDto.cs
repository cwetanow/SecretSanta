namespace SecretSanta.Web.Models.Gift
{
    public class GiftDto
    {
        public GiftDto()
        {

        }

        public GiftDto(SecretSanta.Models.Gift gift)
        {
            if (gift != null)
            {
                this.GroupName = gift.Group.GroupName;
                this.Sender = gift.Sender.UserName;
                this.Receiver = gift.Receiver.UserName;
            }
        }

        public string GroupName { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }
    }
}
