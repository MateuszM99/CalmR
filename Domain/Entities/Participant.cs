using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Participant : BaseEntity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }
        public virtual List<Report> Reports { get; set; }
    }
}