using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.Queries.GetParticipants
{
    public class GetParticipantsQuery : IRequest<List<Participant>>
    {
        public int ConversationId { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<GetParticipantsQuery, List<Participant>>
    {
        private readonly IApplicationDbContext _context;

        public CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Participant>> Handle(GetParticipantsQuery request, CancellationToken cancellationToken)
        {
            var participants = await _context.Participants.Where(p => p.ConversationId == request.ConversationId).Include(u => u.User).ToListAsync(cancellationToken);

            return participants;
        }
    }
}