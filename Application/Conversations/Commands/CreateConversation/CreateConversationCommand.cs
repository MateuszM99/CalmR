using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Conversations.Commands.CreateConversation
{
    public class CreateConversationCommand : IRequest<int>
    {
        public string PsychologistId { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<CreateConversationCommand, int>
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

        public async Task<int> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new ApiException("User not found", StatusCodes.Status404NotFound.ToString());
            }


            var newConversation = new Conversation()
            {
                CreatorId = user.Id,
                CreatedAt = DateTime.Now
            };

            await _context.Conversations.AddAsync(newConversation, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            await _context.Participants.AddAsync(new Participant()
            {
                UserId = user.Id,
                ConversationId = newConversation.Id
            }, cancellationToken);
            
            if (!String.IsNullOrEmpty(request.PsychologistId))
            {
                var psychologistUser = await _userManager.FindByIdAsync(request.PsychologistId);
                
                if (psychologistUser == null)
                {
                    throw new ApiException("Psychologist not found", StatusCodes.Status404NotFound.ToString());
                }
                
                await _context.Participants.AddAsync(new Participant()
                {
                    UserId = psychologistUser.Id,
                    ConversationId = newConversation.Id
                }, cancellationToken);
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            return newConversation.Id;
        }
    }
}