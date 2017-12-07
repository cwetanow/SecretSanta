using System.Threading.Tasks;

namespace SecretSanta.Data.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}
