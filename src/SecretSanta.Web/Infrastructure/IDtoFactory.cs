using System.Collections.Generic;
using SecretSanta.Models;
using SecretSanta.Web.Models.Account;
using SecretSanta.Web.Models.Users;
using SecretSanta.Web.Models.Group;
using SecretSanta.Web.Models.Invite;

namespace SecretSanta.Web.Infrastructure
{
    public interface IDtoFactory
    {
        TokenDto CreateTokenDto(string token);

        UsersListDto CreateUsersListDto(IEnumerable<User> users);

        UserDto CreateUserDto(string username, string email, string displayName);

        GroupDto CreateGroupDto(Group group);

        GroupListDto CreateGroupListDto(IEnumerable<Group> groups);

        InviteListDto CreateInviteListDto(IEnumerable<Invite> invites);

        InviteDto CreateInviteDto(Invite invite);
    }
}
