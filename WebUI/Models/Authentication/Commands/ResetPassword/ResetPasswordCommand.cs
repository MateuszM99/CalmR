using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Helpers;
using CalmR.Models.Authentication.Commands.RequestPasswordReset;
using Infrastructure.Identity.Authentication;
using Infrastructure.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CalmR.Models.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommand : ResetPasswordModel,IRequest<CommandResponse>
    {
        
    }
    
    public class CommandResponse
    {
    }
    
    public class CommandHandler : IRequestHandler<ResetPasswordCommand, CommandResponse>
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly HttpContext _httpContext;

        public CommandHandler(IAuthenticateService authenticateService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._authenticateService = authenticateService ?? throw new ArgumentNullException(nameof(authenticateService));
            this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));

        }
        
        public async Task<CommandResponse> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            CommandResponse response = new CommandResponse();
            await _authenticateService.ResetPassword(command,RequestHelpers.GetIpAddress(_httpContext),RequestHelpers.GetOrigin(_httpContext));
           
            return response;
        }
    }
}