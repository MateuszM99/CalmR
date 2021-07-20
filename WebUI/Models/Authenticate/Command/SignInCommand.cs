using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Identity.Authentication;
using Infrastructure.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CalmR.Models.Authenticate.Command
{
    public class SignInCommand : TokenRequest, IRequest<CommandResponse>
    {
    }
    public class CommandResponse
    {
        public TokenResponse Resource { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<SignInCommand, CommandResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly HttpContext _httpContext;

        public CommandHandler(ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));

        }
        
        public async Task<CommandResponse> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            CommandResponse response = new CommandResponse();

            string ipAddress = _httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            TokenResponse tokenResponse = await _tokenService.Authenticate(command, ipAddress);
            if (tokenResponse == null)
            {
                throw new Exception();
            }

            response.Resource = tokenResponse;
            return response;
        }
    }
}