using Microsoft.EntityFrameworkCore;
using SecretSanta.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecretSanta.Data.Mappings
{
    public class GroupUserMap : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder
                .HasKey(g => g.Id);

            builder
                .HasOne(g => g.User)
                .WithMany(u => u.JoinedGroups)
                .HasForeignKey(g => g.UserId);

            builder
                .HasOne(g => g.Group)
                .WithMany(g=>g.Users)
                .HasForeignKey(g => g.GroupId);
        }
    }
}
