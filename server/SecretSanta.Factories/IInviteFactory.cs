using SecretSanta.Models;
using System;

namespace SecretSanta.Factories
{
    public interface IInviteFactory
    {
        Invite CreateInvite(int groupId, string userId, DateTime date);
    }
}
