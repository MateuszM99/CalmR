using System;
using Domain.Entities;
using Domain.Enums;

namespace Application.Appointments.Queries
{
    public class AppointmentDTO
    {
        public DateTime AppointmentDate { get; set; }
        public int AppointmentDurationTime { get; set; }
        public AppointmentStatus Status { get; set; }
        
        public Psychologist Psychologist { get; set; }
    }
}