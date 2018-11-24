using SecretSanta.Web.Models.Users;
using System;

namespace SecretSanta.Web.Models.Group
{
    public class GroupDto
    {
        public GroupDto()
        {

        }

        public GroupDto(SecretSanta.Models.Group group)
        {
            this.Name = group.GroupName;
            this.Owner = UserDto.FromUser(group.Owner);
        }

        public string Name { get; set; }

        public UserDto Owner { get; set; }

        public static Func<SecretSanta.Models.Group, GroupDto> FromGroup
        {
            get { return (group) => new GroupDto(group); }
        }
    }
}
