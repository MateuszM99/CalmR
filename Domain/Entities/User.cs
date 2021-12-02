using System.Collections.Generic;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public int? PsychologistId { get; set; }
        public virtual Psychologist Psychologist { get; set; }
        public bool IsEnabled { get; set; }
        public string ChatConnectionId { get; set; }
        public virtual List<Participant> Participations { get; set; }
        public virtual List<Conversation> Conversations { get; set; }
        public virtual List<Message> Messages { get; set; }
        public virtual ICollection<Appointment> UserAppointments { get; set; }
        public virtual ICollection<Appointment> PsychologistAppointments { get; set; }
    }
}