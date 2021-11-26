using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Helpers;
using CalmR.Models.Authentication.Commands.RequestPasswordReset;
using Infrastructure.Identity.Authentication;
using Infrastructure.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CalmR.Models.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : ConfirmEmailModel,IRequest<CommandResponse>
    {
        
    }
    
    public class CommandResponse
    {
    }
    
    public class CommandHandler : IRequestHandler<ConfirmEmailCommand, CommandResponse>
    {
        private readonly IAuthenticateService _authenticateService;

        public CommandHandler(IAuthenticateService authenticateService)
        {
            this._authenticateService = authenticateService ?? throw new ArgumentNullException(nameof(authenticateService));
        }
        
        public async Task<CommandResponse> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
        {
            CommandResponse response = new CommandResponse();
            await _authenticateService.ConfirmEmail(command);
           
            return response;
        }
    }
}