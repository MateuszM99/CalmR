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

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new ApiException("Appointment is already canceled", StatusCodes.Status405MethodNotAllowed.ToString());
            }
            
            if (!(appointment.StartDate > now.AddDays(-1)))
            {
                throw new ApiException("You can only cancel appointments 2 days prior appointment start date", StatusCodes.Status405MethodNotAllowed.ToString());
            }

            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync(cancellationToken);

            return new CancelAppointmentResponse()
            {
                IsSuccessful = true,
                Message = "Canceled appointment"
            };
        }
    }
}