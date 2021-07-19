using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserContactConfiguration : IEntityTypeConfiguration<UserContact>
    {
        public void Configure(EntityTypeBuilder<UserContact> builder)
        {
            builder.HasKey(e => new {e.UserId, e.ContactId});

            builder.HasOne(e => e.Contact)
                .WithMany(e => e.UserContacts)
                .HasForeignKey(e => e.ContactId);
        }
    }
}