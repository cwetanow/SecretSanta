using SecretSanta.Models;
using System.Collections.Generic;

namespace SecretSanta.Providers.Contracts
{
    public interface IGiftDistributionProvider
    {
        IEnumerable<Gift> DistributeGifts(IEnumerable<User> groupUsers);
    }
}
