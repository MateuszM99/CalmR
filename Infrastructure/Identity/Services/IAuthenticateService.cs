using System.Threading.Tasks;
using Infrastructure.Identity.Authentication;

namespace Infrastructure.Identity.Services
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, string ipAddress);
    }
}