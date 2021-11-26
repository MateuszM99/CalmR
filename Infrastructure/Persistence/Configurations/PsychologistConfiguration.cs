using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PsychologistConfiguration : IEntityTypeConfiguration<Psychologist>
    {
        public void Configure(EntityTypeBuilder<Psychologist> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(a => a.Address)
                .WithOne(p => p.Psychologist)
                .HasForeignKey<Address>(a => a.PsychologistId);

        }
    }
}