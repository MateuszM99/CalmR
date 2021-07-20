using System.Threading.Tasks;
using Infrastructure.Identity.Authentication;

namespace Infrastructure.Identity.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> Authenticate(TokenRequest request, string ipAddress);
    }
}