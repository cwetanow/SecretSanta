using SecretSanta.Models;
using System.Collections.Generic;

namespace SecretSanta.Providers.Contracts
{
    public interface IGiftManager
    {
        IEnumerable<Gift> DistributeGifts(IList<User> groupUsers, int groupId);
    }
}
