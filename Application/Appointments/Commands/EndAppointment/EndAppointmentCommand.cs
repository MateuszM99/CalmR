using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Appointments.Commands;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Commands.EndAppointment
{ 
    public class EndAppointmentCommand : IRequest<AppointmentStatusChangeResponse>
    {
        public int AppointmentId { get; set; }
    }

    public class CommandHandler : IRequestHandler<EndAppointmentCommand, AppointmentStatusChangeResponse>
    {
        private readonly IApplicationDbContext _context;

        public CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<AppointmentStatusChangeResponse> Handle(EndAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment =
                await _context.Appointments.FirstOrDefaultAsync(x => x.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new ApiException("Appointment not found", StatusCodes.Status404NotFound.ToString());
            }
            
            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                throw new ApiException("Appointment is not confirmed",
                    StatusCodes.Status405MethodNotAllowed.ToString());
            }

            DateTime now = DateTime.Now;

            if (appointment.StartDate.AddHours(appointment.DurationTime) < now)
            {
                throw new ApiException("You can only end appointment after it's due time",
                    StatusCodes.Status405MethodNotAllowed.ToString());
            }

            appointment.Status = AppointmentStatus.Ended;
            await _context.SaveChangesAsync(cancellationToken);

            return new AppointmentStatusChangeResponse()
            {
                IsSuccessful = true,
                Message = "Ended appointment"
            };
        }
    }
}