using System.Collections.Generic;
using Application.Messages.Queries;

namespace Application.Conversations.Queries
{
    public class ConversationDTO
    {
        public int Id { get; set; }
        public List<ConversationUserDTO> ConversationParticipants { get; set; }
        public MessageDTO LastMessage { get; set; }
    }
}