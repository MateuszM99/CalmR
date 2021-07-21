using System;
using System.Threading.Tasks;
using CalmR.Models.Authenticate.Commands;
using CalmR.Models.Authenticate.Commands.SignIn;
using Infrastructure.Identity;
using Infrastructure.Identity.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CalmR.Controllers
{
    public class AuthenticateController : ApiControllerBase
    {
        [HttpPost("signIn")]
        public async Task<TokenResponse> SignInAsync([FromBody]SignInCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
                return response.Resource;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> SignUp()
        {
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail()
        {
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> SendConfirmationEmail()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendPasswordResetLink()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword()
        {
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> ChangePassword()
        {
            return Ok();
        }
    }
}