using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data.Contracts;
using SecretSanta.Data.Mappings;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    public class SecretSantaContext : IdentityDbContext<User>, ISecretSantaContext
    {
        public SecretSantaContext(DbContextOptions<SecretSantaContext> options)
            : base(options)
        {
        }

        public DbSet<Gift> Gifts { get; set; }

        public DbSet<Invite> Invites { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>();
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
        }
    }
}
