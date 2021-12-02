using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Report : BaseEntity
    {
        public bool ReportType { get; set; }
        public string Text { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public int ParticipantId { get; set; }
        public virtual Participant Participant { get; set; }
    }
}