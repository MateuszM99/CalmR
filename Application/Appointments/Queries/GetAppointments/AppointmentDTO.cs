using System;
using Application.Common.DTO;
using Domain.Entities;
using Domain.Enums;

namespace Application.Appointments.Queries
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationTime { get; set; }
        public AppointmentStatus Status { get; set; }
        
        public Psychologist Psychologist { get; set; }
        public UserDTO Patient { get; set; } 
    }
}