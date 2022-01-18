using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Messages.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Messages.Commands
{
    public class AddMessageCommand : IRequest<MessageDTO>
    {
        public int ConversationId { get; set; }
        public long? FileId { get; set; }
        public string Filename { get; set; }
        public string Content { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<AddMessageCommand,MessageDTO>
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

        public async Task<MessageDTO> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                throw new ApiException("User was not found", StatusCodes.Status404NotFound.ToString());
            }

            var newMessage = new Message()
            {
                Status = MessageStatus.Sent,
                Content = request.Content,
                CreatedAt = DateTime.Now,
                SenderId = user.Id,
                FileId = request.FileId,
                ConversationId = request.ConversationId
            };

            await _context.Messages.AddAsync(newMessage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MessageDTO>(newMessage);
        }
    }
}