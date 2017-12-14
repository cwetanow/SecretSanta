using SecretSanta.Models;
using System.Collections.Generic;

namespace SecretSanta.Providers.Contracts
{
    public interface IGiftManager
    {
        IEnumerable<Gift> DistributeGifts(ICollection<User> groupUsers);
    }
}
