using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Psychologists.Queries;
using CalmR.Models.Authenticate.Commands.SignIn;
using Infrastructure.Identity.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CalmR.Controllers
{
    public class PsychologistController : ApiControllerBase
    {
        [Route("get/psychologists")]
        [HttpGet]
        public async Task<List<PsychologistDTO>> List()
        {
            var response = await Mediator.Send(new GetPsychologistsQuery());
            
            return response;
        }
    }
}