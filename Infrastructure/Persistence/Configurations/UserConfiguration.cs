using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(e => e.Messages)
                .WithOne()
                .HasForeignKey(e => e.SenderId);

            builder.HasMany(e => e.Conversations)
                .WithOne()
                .HasForeignKey(e => e.CreatorId);

            builder.HasMany(e => e.Contacts)
                .WithOne()
                .HasForeignKey(e => e.UserId);
        }
    }
}