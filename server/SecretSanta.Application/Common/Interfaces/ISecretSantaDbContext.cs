using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SecretSanta.Domain.Common;

namespace SecretSanta.Application.Common.Interfaces
{
	public interface ISecretSantaDbContext
	{
		public IQueryable<TEntity> Set<TEntity>();

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);

		public void Add<TEntity>(TEntity entity)
			where TEntity : Entity;
	}
}
