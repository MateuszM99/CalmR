using System.Collections.Generic;
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

namespace Application.Appointments.Queries
{
    public class GetUserAppointmentsQuery : IRequest<List<AppointmentDTO>>
    {
        
    }
    
    public class CommandHandler : IRequestHandler<GetUserAppointmentsQuery,List<AppointmentDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDTO>> Handle(GetUserAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);

            if (user == null)
            {
                throw new ApiException("User not found", StatusCodes.Status404NotFound.ToString());
            }

            var appointments = await _context.Appointments
                                                            .Where(a => a.ClientId == user.Id || a.PsychologistId == user.Id)
                                                            .Include(a => a.Psychologist)
                                                            .ToListAsync(cancellationToken);

            if (appointments == null)
            {
                throw new ApiException("No appointments found", StatusCodes.Status404NotFound.ToString());
            }
            
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }
    }
}