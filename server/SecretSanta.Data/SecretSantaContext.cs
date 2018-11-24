using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data.Contracts;
using SecretSanta.Data.Mappings;
using SecretSanta.Models;
using System.Threading.Tasks;

namespace SecretSanta.Data
{
    public class SecretSantaContext : IdentityDbContext<User>, IDbContext
    {
        public SecretSantaContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Gift> Gifts { get; set; }

        public DbSet<Invite> Invites { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupUser> GroupUsers { get; set; }

        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public void SetAdded<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Added;
        }

        public void SetDeleted<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Deleted;
        }

        public void SetUpdated<TEntry>(TEntry entity) where TEntry : class
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Modified;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new GiftMap());
            builder.ApplyConfiguration(new GroupMap());
            builder.ApplyConfiguration(new GroupUserMap());
        }
    }
}
