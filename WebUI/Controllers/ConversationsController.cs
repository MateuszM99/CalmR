using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Conversations.Commands.CreateConversation;
using Application.Conversations.Queries;
using Application.Conversations.Queries.GetConversations;
using Application.Conversations.Queries.GetUserPsychologistConversation;
using Application.Messages.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CalmR.Controllers
{
    public class ConversationsController : ApiControllerBase
    {
        [Route("get/conversations")]
        [HttpGet]
        public async Task<List<ConversationDTO>> GetConversations([FromQuery]GetConversationsQuery query)
        {
            var response = await Mediator.Send(query);
            
            return response;
        }
        
        [Route("get/user-psychologist-conversation")]
        [HttpGet]
        public async Task<ConversationDTO> GetUserPsychologistConversations([FromQuery]GetUserPsychologistConversationQuery query)
        {
            var response = await Mediator.Send(query);
            
            return response;
        }
        
        [Route("create-conversation")]
        [HttpPost]
        public async Task<int> CreateConversation([FromBody] CreateConversationCommand command)
        {
            var response = await Mediator.Send(command);
            
            return response;
        }
        
    }
}