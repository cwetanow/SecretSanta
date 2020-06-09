using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Common;

namespace SecretSanta.Persistence
{
	public class SecretSantaContext : DbContext
	{
		public SecretSantaContext(DbContextOptions<SecretSantaContext> options)
			: base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				// Configure primary key
				if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
				{
					modelBuilder.Entity(entityType.ClrType)
						.HasKey(nameof(Entity.Id));
				}
			}

			// Disable cascade delete
			modelBuilder.Model.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
				.ToList()
				.ForEach(e => e.DeleteBehavior = DeleteBehavior.Restrict);

			modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
