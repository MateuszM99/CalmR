using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Appointments.Queries;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Psychologists.Queries
{
    public class GetPsychologistsQuery : IRequest<List<PsychologistDTO>>
    {
        
    }
    
    public class CommandHandler : IRequestHandler<GetPsychologistsQuery,List<PsychologistDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PsychologistDTO>> Handle(GetPsychologistsQuery request, CancellationToken cancellationToken)
        {
            var psychologists = await _context.Psychologists.Include(p => p.User).ToListAsync(cancellationToken);

            if (psychologists == null)
            {
                throw new ApiException("No psychologists found", StatusCodes.Status404NotFound.ToString());
            }

            return _mapper.Map<List<PsychologistDTO>>(psychologists);
        }
    }
}