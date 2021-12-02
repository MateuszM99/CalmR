using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommand : IRequest<int>
    {
        public DateTime AppointmentDate { get; set; }
        public int AppointmentDurationTime { get; set; }
        public string PsychologistId { get; set; }
    }

    public class CommandHandler : IRequestHandler<CreateAppointmentCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;

        public CommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }


        public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new ApiException("User was not found", StatusCodes.Status404NotFound.ToString());
            }

            var psychologistAppointments = await _context.Appointments
                                                                        .Where(a => a.PsychologistId == request.PsychologistId)
                                                                        .ToListAsync(cancellationToken);

            if (psychologistAppointments.Any(a => a.StartDate.AddHours(a.DurationTime) < request.AppointmentDate))
            {
                throw new ApiException("There is already appointment made for this time",StatusCodes.Status405MethodNotAllowed.ToString());
            }
            
            Appointment newAppointment = new Appointment()
            {
                StartDate = request.AppointmentDate,
                DurationTime = request.AppointmentDurationTime,
                Status = AppointmentStatus.New,
                PsychologistId = request.PsychologistId,
                ClientId = user.Id
            };

            await _context.Appointments.AddAsync(newAppointment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return newAppointment.Id;
        }
    }
}