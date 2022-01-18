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

namespace Application.Messages.Queries
{
    public class GetMessagesQuery : IRequest<List<MessageDTO>>
    {
        public int? ConversationId { get; set; }
        
    }

    public class CommandHandler : IRequestHandler<GetMessagesQuery, List<MessageDTO>>
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

        public async Task<List<MessageDTO>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new ApiException("User was not found", StatusCodes.Status404NotFound.ToString());
            }

            var isParticipant = await _context.Conversations.Where(c => c.Id == request.ConversationId)
                                                            .AnyAsync(c => c.Participants.Select(p => p.User).Contains(user), cancellationToken);

            if (!isParticipant)
            {
                throw new ApiException("Cannot load this conversation", StatusCodes.Status405MethodNotAllowed.ToString());
            }
            
            var query = _context.Messages.Include(m => m.Sender).Include(f => f.File).AsQueryable();

            query = ApplyFilter(query, request);

            var messages = await query.ToListAsync(cancellationToken);

            return _mapper.Map<List<MessageDTO>>(messages);
        }

        private IQueryable<Message> ApplyFilter(IQueryable<Message> query, GetMessagesQuery request)
        {
            if (request.ConversationId != null)
            {
                query = query.Where(m => m.ConversationId == request.ConversationId);
            }

            return query;
        }
    }
}