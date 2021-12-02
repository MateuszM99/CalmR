using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Appointments.Commands.CancelAppointment;
using Application.Appointments.Commands.CreateAppointment;
using Application.Appointments.Commands.UpdateAppointment;
using Application.Appointments.Queries;
using Application.Appointments.Queries.GetAvailableHours;
using Application.Psychologists.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CalmR.Controllers
{
    public class AppointmentsController : ApiControllerBase
    {
        [Route("get/appointments")]
        [HttpGet]
        public async Task<List<AppointmentDTO>> GetAppointments()
        {
            var response = await Mediator.Send(new GetUserAppointmentsQuery());
            
            return response;
        }
        
        [Route("create-appointment")]
        [HttpPost]
        public async Task<int> CreateAppointment([FromBody] CreateAppointmentCommand command)
        {
            var response = await Mediator.Send(command);
            
            return response;
        }
        
        [Route("cancel-appointment")]
        [HttpPost]
        public async Task<CancelAppointmentResponse> CancelAppointment([FromBody] CancelAppointmentCommand command)
        {
            var response = await Mediator.Send(command);
            
            return response;
        }
        
        [Route("update-appointment")]
        [HttpPost]
        public async Task<int> UpdateAppointment([FromBody] UpdateAppointmentCommand command)
        {
            var response = await Mediator.Send(command);
            
            return response;
        }
        
        [Route("get/appointments-available-hours")]
        [HttpGet]
        public async Task<List<DateTime>> GetAvailableHoursForGivenDate([FromQuery] GetAvailableHoursQuery command)
        {
            var response = await Mediator.Send(command);
            
            return response;
        }
    }
}