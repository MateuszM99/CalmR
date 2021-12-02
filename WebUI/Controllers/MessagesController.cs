using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Appointments.Queries;
using Application.Messages.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CalmR.Controllers
{
    public class MessagesController : ApiControllerBase
    {
        [Route("get/messages")]
        [HttpGet]
        public async Task<List<MessageDTO>> GetMessages([FromQuery]GetMessagesQuery query)
        {
            var response = await Mediator.Send(query);
            
            return response;
        }
    }
}