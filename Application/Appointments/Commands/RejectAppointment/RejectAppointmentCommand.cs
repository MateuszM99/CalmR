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

namespace Application.Appointments.Commands.RejectAppointment
{ 
    public class RejectAppointmentCommand : IRequest<AppointmentStatusChangeResponse>
    {
        public int AppointmentId { get; set; }
    }

    public class CommandHandler : IRequestHandler<RejectAppointmentCommand, AppointmentStatusChangeResponse>
    {
        private readonly IApplicationDbContext _context;

        public CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<AppointmentStatusChangeResponse> Handle(RejectAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment =
                await _context.Appointments.FirstOrDefaultAsync(x => x.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new ApiException("Appointment not found", StatusCodes.Status404NotFound.ToString());
            }

            
            if (appointment.Status != AppointmentStatus.Rejected)
            {
                throw new ApiException("Appointment is not in a status to be rejected",
                    StatusCodes.Status405MethodNotAllowed.ToString());
            }
            
            appointment.Status = AppointmentStatus.Rejected;
            await _context.SaveChangesAsync(cancellationToken);

            return new AppointmentStatusChangeResponse()
            {
                IsSuccessful = true,
                Message = "Rejected appointment"
            };
        }
    }
}