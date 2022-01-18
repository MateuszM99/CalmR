using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Psychologist> Psychologists { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<File> Files { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}