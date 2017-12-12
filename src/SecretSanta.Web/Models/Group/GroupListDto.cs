using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Web.Models.Group
{
    public class GroupListDto
    {
        public GroupListDto()
        {
            this.Groups = new List<GroupDto>();
        }

        public GroupListDto(IEnumerable<SecretSanta.Models.Group> groups)
        {
            this.Groups = groups
                .Select(GroupDto.FromGroup)
                .ToList();
        }

        public IEnumerable<GroupDto> Groups { get; set; }
    }
}
