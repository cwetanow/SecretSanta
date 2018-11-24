using System.Collections.Generic;
using System.Linq;
using SecretSanta.Models;

namespace SecretSanta.Web.Models.Users
{
    public class UsersListDto
    {
        public UsersListDto()
        {
            this.Users = new List<UserDto>();
        }

        public UsersListDto(IEnumerable<User> users) : this()
        {
            this.Users = users
                .Select(UserDto.FromUser)
                .ToList();
        }

        public IEnumerable<UserDto> Users { get; set; }
    }
}
