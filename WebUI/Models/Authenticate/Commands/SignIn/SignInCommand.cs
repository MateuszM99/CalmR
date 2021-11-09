using System;
using System.Threading;
using System.Threading.Tasks;
using CalmR.Filters;
using Infrastructure.Identity.Authentication;
using Infrastructure.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CalmR.Models.Authenticate.Commands.SignIn
{
    public class SignInCommand : AuthenticateRequest, IRequest<CommandResponse>
    {
    }
    public class CommandResponse
    {
        public AuthenticateResponse Resource { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<SignInCommand, CommandResponse>
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly HttpContext _httpContext;

        public CommandHandler(IAuthenticateService authenticateService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._authenticateService = authenticateService ?? throw new ArgumentNullException(nameof(authenticateService));
            this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));

        }
        
        public async Task<CommandResponse> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            CommandResponse response = new CommandResponse();

            string ipAddress = _httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            AuthenticateResponse authenticateResponse = await _authenticateService.Authenticate(command, ipAddress);
            if (authenticateResponse == null)
            {
                throw new ApiException("Blad",StatusCodes.Status401Unauthorized.ToString());
            }

            response.Resource = authenticateResponse;
            return response;
        }
    }
}