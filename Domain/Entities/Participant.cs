using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Participant : BaseEntity
    {
        public string UserId { get; set; }
        public int ConversationId { get; set; }
        
        public virtual Conversation Conversation { get; set; }
    }
}