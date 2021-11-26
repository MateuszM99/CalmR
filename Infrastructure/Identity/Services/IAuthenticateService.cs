using System.Threading.Tasks;
using Infrastructure.Identity.Authentication;

namespace Infrastructure.Identity.Services
{
    public interface IAuthenticateService
    {
        Task<SignUpResponse> SignUp(SignUpRequest request, string ipAddress, string origin);
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, string ipAddress);
        Task ConfirmEmail(ConfirmEmailModel model);
        Task ResendConfirmationEmail(string userId, string origin);
        Task RequestPasswordReset(ResetPasswordRequest request, string ipAddress, string origin);
        Task ResetPassword(ResetPasswordModel model, string ipAddress, string origin);
    }
}