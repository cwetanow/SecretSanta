using SecretSanta.Models;
using System.Threading.Tasks;

namespace SecretSanta.Services.Contracts
{
    public interface IGroupService
    {
        Task<Group> CreateGroupAsync(string groupName, string ownerId);

        Group GetByName(string groupName);
    }
}
