using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Application.Conversations.Queries.GetConversations;
using Application.Files.Commands.AddFile;
using Application.Messages.Commands;
using Application.Messages.Queries;
using Application.Participants.Queries.GetParticipants;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public ChatHub(ICurrentUserService currentUserService, IMediator mediator, UserManager<User> userManager, IApplicationDbContext context)
        {
            _currentUserService = currentUserService;
            _mediator = mediator;
            _userManager = userManager;
            _context = context;
        }

        public async Task SendMessage(AddMessageCommand command)
        {
            var message = await _mediator.Send(command);

            message.FileName = command.Filename;
            message.FileId = command.FileId;
            
            var conversation = (await _mediator.Send(new GetConversationsQuery()
            {
                ConversationId = command.ConversationId
            })).FirstOrDefault();

            if (conversation != null)
            {
                conversation.LastMessage = message;
            }

            await Clients.All.SendAsync("UpdateConversation", conversation);
            
            await Clients.Group(command.ConversationId.ToString()).SendAsync("ReceiveMessage", message);
        }

        public async Task AddToConversation(string ConversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ConversationId);
        }

        public async Task RemoveFromConversation(string ConversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ConversationId);
        }

        public async Task UpdateConversation(int ConversationId)
        {
            
        }


        public override async Task OnConnectedAsync()
        {
            //Context.QueryString
            //ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
            var connectionId = Context.ConnectionId;
            User currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            currentUser.ChatConnectionId = connectionId;

            await _userManager.UpdateAsync(currentUser);

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            
            var connectedUser = await _context.Users.SingleAsync(x => x.ChatConnectionId == connectionId);
            if (connectedUser != null)
            {
                connectedUser.ChatConnectionId = null;
            }

            await _context.SaveChangesAsync(CancellationToken.None);

            await base.OnDisconnectedAsync(exception);
        }
        
    }
}