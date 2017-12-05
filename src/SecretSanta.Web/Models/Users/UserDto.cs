using System;
using SecretSanta.Models;

namespace SecretSanta.Web.Models.Users
{
    public class UserDto
    {
        public UserDto(string username, string email, string displayName)
        {
            this.UserName = username;
            this.Email = email;
            this.DisplayName = displayName;
        }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public static Func<User, UserDto> FromUser
        {
            get { return (user) => new UserDto(user.UserName, user.Email, user.DisplayName); }
        }
    }
}
