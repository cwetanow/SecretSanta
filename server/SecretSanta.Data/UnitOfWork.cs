using System.Threading.Tasks;
using SecretSanta.Data.Contracts;
using System;

namespace SecretSanta.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext dbContext;

        public UnitOfWork(IDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            this.dbContext = dbContext;
        }

        public Task<int> CommitAsync()
        {
            return this.dbContext.SaveChangesAsync();
        }
    }
}
