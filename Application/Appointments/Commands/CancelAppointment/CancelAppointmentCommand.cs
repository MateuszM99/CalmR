using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentCommand : IRequest<CancelAppointmentResponse>
    {
        public int AppointmentId { get; set; }
    }

    public class CommandHandler : IRequestHandler<CancelAppointmentCommand,CancelAppointmentResponse>
    {
        private readonly IApplicationDbContext _context;

        public CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<CancelAppointmentResponse> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new ApiException("Appointment not found", StatusCodes.Status404NotFound.ToString());
            }
            
            DateTime now = DateTime.Now;

            if (appointment.AppointmentDate > now.AddDays(-1))
            {
                appointment.Status = AppointmentStatus.Cancelled;
                await _context.SaveChangesAsync(cancellationToken);

                return new CancelAppointmentResponse()
                {
                    IsSuccessful = true,
                    Message = "Canceled appointment"
                };
            }

            return new CancelAppointmentResponse()
            {
                IsSuccessful = false,
                Message =
                    "Couldn't cancel appointment, you can only cancel appointment 2 days prior the appointment date"
            };
        }
    }
}