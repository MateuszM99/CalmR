using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Queries.GetUpcomingAppointment
{
    public class GetUpcomingAppointmentQuery : IRequest<AppointmentDTO>
    {
        
    }

    public class CommandHandler : IRequestHandler<GetUpcomingAppointmentQuery, AppointmentDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CommandHandler(ICurrentUserService currentUser, UserManager<User> userManager, IApplicationDbContext context, IMapper mapper)
        {
            _currentUser = currentUser;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppointmentDTO> Handle(GetUpcomingAppointmentQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);

            if (user == null)
            {
                throw new ApiException("User not found", StatusCodes.Status404NotFound.ToString());
            }

            var upcomingAppointment = await _context.Appointments
                                            .Where(a => (a.ClientId == user.Id || a.PsychologistId == user.Id) && a.StartDate.AddHours(a.DurationTime) >= DateTime.Now)
                                            .Include(a => a.Psychologist)
                                            .ThenInclude(u => u.Psychologist)
                                            .ThenInclude(p => p.Address)
                                            .Include(a => a.Client)
                                            .OrderByDescending(a => a.StartDate)
                                            .FirstOrDefaultAsync(cancellationToken);
            
            if (upcomingAppointment == null)
            {
                throw new ApiException("No upcoming appointment", StatusCodes.Status404NotFound.ToString());
            }

            return _mapper.Map<AppointmentDTO>(upcomingAppointment);
        }
    }
}