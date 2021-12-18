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

namespace Application.Conversations.Queries.GetConversations
{
    public class GetConversationsQuery : IRequest<List<ConversationDTO>>
    {
        public int? ConversationId { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<GetConversationsQuery,List<ConversationDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<ConversationDTO>> Handle(GetConversationsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new ApiException("User not found", StatusCodes.Status404NotFound.ToString());
            }

            var query = _context.Conversations.Where(c => c.Participants.Any(p => p.UserId == user.Id)).AsQueryable();
            query = ApplyFilter(request, query);

            var conversations = await query.Include(c => c.Messages)
                                                        .Include(u => u.Participants)
                                                        .ThenInclude(p => p.User)
                                                        .ThenInclude(u => u.Psychologist)
                                                        .ToListAsync(cancellationToken);

            return _mapper.Map<List<ConversationDTO>>(conversations);
        }

        private IQueryable<Conversation> ApplyFilter(GetConversationsQuery request, IQueryable<Conversation> query)
        {
            if (request.ConversationId != null)
            {
                query = query.Where(c => c.Id == request.ConversationId);
            }

            return query;
        }
    }
}