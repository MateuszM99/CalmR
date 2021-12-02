using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.Messages)
                .WithOne(e => e.Conversation)
                .HasForeignKey(e => e.ConversationId);

            builder.HasMany(e => e.Participants)
                .WithOne(e => e.Conversation)
                .HasForeignKey(e => e.ConversationId);

            builder.HasOne(e => e.Creator)
                .WithMany(e => e.Conversations)
                .HasForeignKey(e => e.CreatorId);
        }
    }
}