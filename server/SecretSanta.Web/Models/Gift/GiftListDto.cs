using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Web.Models.Gift
{
    public class GiftListDto
    {
        public GiftListDto()
        {
            this.Gifts = new List<GiftDto>();
        }

        public GiftListDto(IEnumerable<SecretSanta.Models.Gift> gifts)
        {
            this.Gifts = gifts
                .Select(GiftDto.FromGift)
                .ToList();
        }

        public IEnumerable<GiftDto> Gifts { get; set; }
    }
}
