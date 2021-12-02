using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(e => e.Messages)
                .WithOne(e => e.Sender)
                .HasForeignKey(e => e.SenderId);

            builder.HasMany(e => e.Participations)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            builder.HasMany(e => e.Conversations)
                .WithOne(e => e.Creator)
                .HasForeignKey(e => e.CreatorId);
        }
    }
}