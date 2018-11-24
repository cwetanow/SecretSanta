using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SecretSanta.Data.Contracts
{
    public interface IDbContext
    {
        DbSet<TEntity> DbSet<TEntity>()
            where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void SetAdded<TEntry>(TEntry entity)
            where TEntry : class;

        void SetDeleted<TEntry>(TEntry entity)
            where TEntry : class;

        void SetUpdated<TEntry>(TEntry entity)
            where TEntry : class;
    }
}
