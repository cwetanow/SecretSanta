namespace SecretSanta.Web.Models.Gift
{
    public class HasGiftDto
    {
        public HasGiftDto()
        {

        }

        public HasGiftDto(bool hasGift, GiftDto giftDto)
        {
            this.HasGift = hasGift;
            this.Gift = giftDto;
        }

        public bool HasGift { get; set; }

        public GiftDto Gift { get; set; }
    }
}
