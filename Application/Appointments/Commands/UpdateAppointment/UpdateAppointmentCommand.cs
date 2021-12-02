using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommand : IRequest<int>
    {
        public DateTime AppointmentDate { get; set; }
        public int AppointmentDurationTime { get; set; }
        public int AppointmentId { get; set; }
    }

    public class CommandHandler : IRequestHandler<UpdateAppointmentCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new ApiException("Appointment was not found", StatusCodes.Status404NotFound.ToString());
            }
            
            var psychologistAppointments = await _context.Appointments
                .Where(a => a.PsychologistId == appointment.PsychologistId)
                .ToListAsync(cancellationToken);

            if (psychologistAppointments.Any(a => a.StartDate.AddHours(a.DurationTime) < request.AppointmentDate))
            {
                throw new ApiException("There is already appointment made for this time",StatusCodes.Status405MethodNotAllowed.ToString());
            }

            appointment.StartDate = request.AppointmentDate;
            appointment.DurationTime = request.AppointmentDurationTime;
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return appointment.Id;
        }
    }
}