using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using CalmR.Models.Authenticate.Commands.SignIn;
using CalmR.Models.Authenticate.Commands.SignUp;
using Infrastructure.Identity.Authentication;
using Infrastructure.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CalmR.Models.Authentication.Commands.ResendConfirmationEmail
{
    public class ResendConfirmationEmailCommand : IRequest<CommandResponse>
    {
        public string Email { get; set; }
    }
    
    public class CommandResponse
    {
    }
    
    public class CommandHandler : IRequestHandler<ResendConfirmationEmailCommand, CommandResponse>
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly HttpContext _httpContext;

        public CommandHandler(IAuthenticateService authenticateService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._authenticateService = authenticateService ?? throw new ArgumentNullException(nameof(authenticateService));
            this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));

        }
        
        public async Task<CommandResponse> Handle(ResendConfirmationEmailCommand command, CancellationToken cancellationToken)
        {
            CommandResponse response = new CommandResponse();

            await _authenticateService.ResendConfirmationEmail(command.Email, RequestHelpers.GetOrigin(_httpContext));

            return response;
        }
    }
}