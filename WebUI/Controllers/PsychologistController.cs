using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.DTO;
using Application.Psychologists.Commands.EditPsychologistProfile;
using Application.Psychologists.Queries;
using Application.Psychologists.Queries.GetPsychologistProfile;
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
        
        [Route("get/psychologistProfile")]
        [HttpGet]
        public async Task<PsychologistDTO> GetPsychologistProfile()
        {

            var response = await Mediator.Send(new GetPsychologistProfileQuery());
            
            return response;
        }
        
        [Route("editPsychologistProfile")]
        [HttpPost]
        public async Task<PsychologistDTO> EditPsychologistProfile([FromForm]EditPsychologistProfileCommand command)
        {

            var response = await Mediator.Send(command);
            
            return response;
        }
    }
}