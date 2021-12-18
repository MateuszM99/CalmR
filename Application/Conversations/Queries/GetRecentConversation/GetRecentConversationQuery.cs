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

namespace Application.Conversations.Queries.GetRecentConversation
{
    public class GetRecentConversationQuery : IRequest<int?>
    {
        
    }
    
    public class CommandHandler : IRequestHandler<GetRecentConversationQuery,int?>
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

        public async Task<int?> Handle(GetRecentConversationQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new ApiException("User not found", StatusCodes.Status404NotFound.ToString());
            }

            var recentConversationId = await _context.Conversations.Where(c => c.Participants.Any(p => p.UserId == user.Id)).OrderByDescending(c => c.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()).Select(c => c.Id).FirstOrDefaultAsync(cancellationToken);

            return recentConversationId;
        }
    }
}