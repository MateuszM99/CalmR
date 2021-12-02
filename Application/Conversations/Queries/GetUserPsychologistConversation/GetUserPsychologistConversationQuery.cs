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

namespace Application.Conversations.Queries.GetUserPsychologistConversation
{
    public class GetUserPsychologistConversationQuery : IRequest<ConversationDTO>
    {
        public string PsychologistId { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<GetUserPsychologistConversationQuery, ConversationDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CommandHandler(ICurrentUserService currentUserService, IApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ConversationDTO> Handle(GetUserPsychologistConversationQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new ApiException("User not found", StatusCodes.Status404NotFound.ToString());
            }

            var psychologistUser = await _userManager.FindByIdAsync(request.PsychologistId);

            if (psychologistUser == null)
            {
                throw new ApiException("Psychologist not found", StatusCodes.Status404NotFound.ToString());
            }

            var conversation = await _context.Conversations.Where(c =>
                                                                c.Participants.Any(p => p.UserId == user.Id) &&
                                                                c.Participants.Any(p => p.UserId == psychologistUser.Id) &&
                                                                c.Participants.Count == 2)
                                                            .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<ConversationDTO>(conversation);
        }
    }
}