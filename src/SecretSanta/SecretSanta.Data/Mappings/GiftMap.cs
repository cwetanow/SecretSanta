using Microsoft.EntityFrameworkCore;
using SecretSanta.Models;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecretSanta.Data.Mappings
{
    public class GiftMap : IEntityTypeConfiguration<Gift>
    {
        public void Configure(EntityTypeBuilder<Gift> builder)
        {
            builder
                .HasKey(g => g.Id);

            builder
                .HasOne(g => g.Sender)
                .WithMany(u => u.SentGifts)
                .HasForeignKey(g => g.SenderId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;

            builder
                .HasOne(g => g.Receiver)
                .WithMany(u => u.ReceivedGifts)
                .HasForeignKey(g => g.ReceiverId)
                .Metadata.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
