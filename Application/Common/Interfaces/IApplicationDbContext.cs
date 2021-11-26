using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Psychologist> Psychologists { get; set; }
        public DbSet<Address> Addresses { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}