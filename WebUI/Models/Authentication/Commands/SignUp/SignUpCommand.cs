using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Infrastructure.Identity.Authentication;
using Infrastructure.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CalmR.Models.Authenticate.Commands.SignUp
{
    public class SignUpCommand : SignUpRequest, IRequest<CommandResponse>
    {
        
    }

    public class CommandResponse
    {
        public SignUpResponse Resource { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<SignUpCommand,CommandResponse> {
        private readonly IAuthenticateService _authenticateService;
        private readonly HttpContext _httpContext;

        public CommandHandler(IAuthenticateService authenticateService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._authenticateService = authenticateService ?? throw new ArgumentNullException(nameof(authenticateService));
            this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        
        public async Task<CommandResponse> Handle(SignUpCommand command, CancellationToken cancellationToken)
        {
            CommandResponse response = new CommandResponse();

            SignUpResponse signUpResponse = await _authenticateService.SignUp(command, RequestHelpers.GetIpAddress(_httpContext), RequestHelpers.GetOrigin(_httpContext));
            if (!signUpResponse.Succeeded)
            {
                throw new ApiException("Something went wrong",StatusCodes.Status500InternalServerError.ToString());
            }

            response.Resource = signUpResponse;
            return response;
        }
    }
}