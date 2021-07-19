using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Conversation : BaseEntity
    {
        public string Title { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public virtual List<Message> Messages { get; set; }
        public virtual List<Participant> Participants { get; set; }
    }
}