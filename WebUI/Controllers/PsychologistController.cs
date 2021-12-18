using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.DTO;
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
        public async Task<PagedResult<PsychologistDTO>> List([FromQuery] PageDTO pageDto, [FromQuery] string s)
        {
            
            var response = await Mediator.Send(new GetPsychologistsQuery()
            {
                pageDto = pageDto,
                textSearch = s,
            });
            
            return response;
        }
    }
}