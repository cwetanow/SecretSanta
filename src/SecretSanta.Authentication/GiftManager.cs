using System.Collections.Generic;
using SecretSanta.Models;
using SecretSanta.Providers.Contracts;
using SecretSanta.Infrastructure.Extensions;
using SecretSanta.Factories;
using System.Linq;

namespace SecretSanta.Authentication
{
    public class GiftManager : IGiftManager
    {
        private readonly IGiftFactory factory;

        public GiftManager(IGiftFactory factory)
        {
            this.factory = factory;
        }

        public IEnumerable<Gift> DistributeGifts(IList<User> groupUsers, int groupId)
        {
            var count = groupUsers.Count;

            var distribution = this.SecretSantaShuffle(count);

            var gifts = distribution
                  .Select((element, index) =>
                  {
                      var senderId = groupUsers[element].Id;
                      var receiverId = groupUsers[element].Id;

                      var gift = this.factory.CreateGift(groupId, senderId, receiverId);

                      return gift;
                  })
                  .ToList();

            return gifts;
        }

        private IList<int> SecretSantaShuffle(int numberOfUsers)
        {
            var assignments = new List<int>();
            for (int i = 0; i < numberOfUsers; i++)
            {
                assignments[i] = i;
            }

            var isValid = false;

            while (!isValid)
            {
                assignments.Shuffle();

                isValid = true;

                for (int i = 0; i < numberOfUsers; i++)
                {
                    if (assignments[i] == i)
                    {
                        isValid = false;
                    }
                }
            }

            return assignments;
        }
    }
}
