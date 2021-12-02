using System;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Message : BaseEntity
    {
        public MessageStatus Status { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string SenderId { get; set; }
        public virtual User Sender { get; set; }
        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}