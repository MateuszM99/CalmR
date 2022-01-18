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

namespace Application.Psychologists.Queries.GetPsychologistProfile
{
    public class GetPsychologistProfileQuery : IRequest<PsychologistDTO>
    {
    }

    public class CommandHandler : IRequestHandler<GetPsychologistProfileQuery, PsychologistDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;

        public CommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<PsychologistDTO> Handle(GetPsychologistProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new ApiException("User was not found", StatusCodes.Status404NotFound.ToString());
            }

            if (user.PsychologistId == null)
            {
                throw new ApiException("User is not a psychologist", StatusCodes.Status404NotFound.ToString());
            }
            
            var psychologist = await _context.Psychologists.Include(u => u.User).Include(a => a.Address)
                .FirstOrDefaultAsync(x => x.Id == user.PsychologistId, cancellationToken);

            if (psychologist == null)
            {
                throw new ApiException("No psychologist found", StatusCodes.Status404NotFound.ToString());
            }

            return _mapper.Map<PsychologistDTO>(psychologist);
        }
    }
}