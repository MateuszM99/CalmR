using System;
using System.Threading.Tasks;
using CalmR.Models.Authenticate.Commands;
using CalmR.Models.Authenticate.Commands.SignIn;
using CalmR.Models.Authenticate.Commands.SignUp;
using CalmR.Models.Authentication.Commands.ConfirmEmail;
using CalmR.Models.Authentication.Commands.RequestPasswordReset;
using CalmR.Models.Authentication.Commands.ResendConfirmationEmail;
using CalmR.Models.Authentication.Commands.ResetPassword;
using CalmR.Models.Authentication.Commands.SignUpPsychologist;
using Infrastructure.Identity;
using Infrastructure.Identity.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CalmR.Controllers
{
    public class AuthenticateController : ApiControllerBase
    {
        [Route("signIn")]
        [HttpPost]
        public async Task<AuthenticateResponse> SignIn([FromBody]SignInCommand command)
        {
            var response = await Mediator.Send(command);
            return response.Resource;
        }

        [Route("signUp")]
        [HttpPost]
        public async Task<SignUpResponse> SignUp([FromBody] SignUpCommand command)
        {
            var response = await Mediator.Send(command);

            return response.Resource;
        }
        
        [Route("signUp-psychologist")]
        [HttpPost]
        public async Task<SignUpResponse> SignUpPsychologist([FromForm] SignUpPsychologistCommand command)
        {
            var response = await Mediator.Send(command);

            return response;
        }

        [Route("confirm")]
        [HttpPost]
        public async Task ConfirmEmail([FromBody] ConfirmEmailCommand command) => await Mediator.Send(command); 
        
        [Route("resend-confirm")]
        [HttpPost]
        public async Task ResendConfirmationEmail([FromBody] ResendConfirmationEmailCommand command) => await Mediator.Send(command);

        [Route("request-reset")]
        [HttpPost]
        public async Task RequestPasswordReset([FromBody] RequestPasswordResetCommand command) => await Mediator.Send(command);

        [Route("reset")]
        [HttpPost]
        public async Task ResetPassword([FromBody] ResetPasswordCommand command) => await Mediator.Send(command);
        
    }
}