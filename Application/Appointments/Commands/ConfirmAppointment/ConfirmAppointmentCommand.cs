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

namespace Application.Appointments.Commands.ConfirmAppointment
{
    public class ConfirmAppointmentCommand : IRequest<AppointmentStatusChangeResponse>
    {
        public int AppointmentId { get; set; }
    }

    public class CommandHandler : IRequestHandler<ConfirmAppointmentCommand, AppointmentStatusChangeResponse>
    {
        private readonly IApplicationDbContext _context;

        public CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<AppointmentStatusChangeResponse> Handle(ConfirmAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var appointment =
                await _context.Appointments.FirstOrDefaultAsync(x => x.Id == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new ApiException("Appointment not found", StatusCodes.Status404NotFound.ToString());
            }
            

            if (appointment.Status != AppointmentStatus.AwaitingForConfirmation)
            {
                throw new ApiException("Appointment is not awaiting for confirmation",
                    StatusCodes.Status405MethodNotAllowed.ToString());
            }

            appointment.Status = AppointmentStatus.Confirmed;
            await _context.SaveChangesAsync(cancellationToken);

            return new AppointmentStatusChangeResponse()
            {
                IsSuccessful = true,
                Message = "Appointment confirmed"
            };
        }
    }
}