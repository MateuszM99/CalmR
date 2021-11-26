using System;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentDate { get; set; }
        public int AppointmentDurationTime { get; set; }
        public AppointmentStatus Status { get; set; }
        
        public string PsychologistId { get; set; }
        public virtual User Psychologist { get; set; } 
        public string ClientId { get; set; }
        public virtual User Client { get; set; }
    }
}