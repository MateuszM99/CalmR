using System.Threading.Tasks;
using Application.Appointments.Queries;
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

        public VideoChatHub(UserManager<User> userManager, ICurrentUserService currentUserService, IApplicationDbContext context)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task JoinMeeting(AppointmentDTO appointmentDto)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, appointmentDto.Id.ToString());

            await Clients.Group(appointmentDto.Id.ToString()).SendAsync("UserConnected");
        }

        public async Task LeaveMeeting(AppointmentDTO appointmentDto)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appointmentDto.Id.ToString());

            await Clients.Group(appointmentDto.Id.ToString()).SendAsync("UserDisconnected");
        }

        public async Task EndMeeting(AppointmentDTO appointmentDto)
        {
            
        }
    }
}