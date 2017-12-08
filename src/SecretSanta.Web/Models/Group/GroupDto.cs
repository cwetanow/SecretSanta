using SecretSanta.Web.Models.Users;

namespace SecretSanta.Web.Models.Group
{
    public class GroupDto
    {
        public GroupDto(SecretSanta.Models.Group group)
        {
            this.Name = group.GroupName;
            this.Owner = UserDto.FromUser(group.Owner);
        }

        public string Name { get; set; }

        public UserDto Owner { get; set; }
    }
}
