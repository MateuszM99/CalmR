using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Appointments.Queries;
using Application.Common.DTO;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    public class VideoChatHub : Hub
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IApplicationDbContext _context;
        private static ConcurrentDictionary<int, string> Users = new ConcurrentDictionary<int, string>();

        public VideoChatHub(UserManager<User> userManager, ICurrentUserService currentUserService, IApplicationDbContext context)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task JoinMeeting(VideoChatDTO chatDto)
        {
            bool isUserConnected = Users.Any(c => c.Key == chatDto.AppointmentId);
            Users.TryAdd(chatDto.AppointmentId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatDto.AppointmentId.ToString());

            await Clients.GroupExcept(chatDto.AppointmentId.ToString(), Context.ConnectionId).SendAsync("JoinedMeeting", new VideoChatResponseDTO()
            {
                isUserConnected = isUserConnected
            });
        }

        public async Task SendSignal(VideoChatDTO chatDto)
        {
            await Clients.GroupExcept(chatDto.AppointmentId.ToString(), Context.ConnectionId).SendAsync("UserJoined", chatDto.PeerSignal);
        }

        public async Task ReturnSignal(VideoChatDTO chatDto)
        {
            await Clients.GroupExcept(chatDto.AppointmentId.ToString(), Context.ConnectionId).SendAsync("ReceiveReturnedSignal", chatDto.PeerSignal);
        }

        public async Task LeaveMeeting(VideoChatDTO chatDto)
        {
            Users.TryRemove(chatDto.AppointmentId, out string connection);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatDto.AppointmentId.ToString());

            await Clients.GroupExcept(chatDto.AppointmentId.ToString(), Context.ConnectionId).SendAsync("UserDisconnected");
        }

        public async Task EndMeeting(VideoChatDTO chatDto)
        {
            Users.TryRemove(chatDto.AppointmentId, out _);

            await Clients.Group(chatDto.AppointmentId.ToString()).SendAsync("MeetingEnded");
        }
    }
}