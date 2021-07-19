using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Message : BaseEntity
    {
        public int ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public virtual Conversation Conversation { get; set; }
    }
}