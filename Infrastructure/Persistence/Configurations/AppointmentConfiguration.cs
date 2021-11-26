using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(c => c.Client)
                .WithMany(c => c.UserAppointments)
                .HasForeignKey(c => c.ClientId)
                .HasPrincipalKey(a => a.Id);

            builder.HasOne(p => p.Psychologist)
                .WithMany(c => c.PsychologistAppointments)
                .HasForeignKey(c => c.PsychologistId)
                .HasPrincipalKey(a => a.Id);


        }
    }
}