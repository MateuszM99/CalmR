using Domain.Entities;

namespace Infrastructure.Identity.Authentication
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(User user, 
            string role, 
            string token
        )
        {
            Id = user.Id;
            EmailAddress = user.Email;
            Token = token;
            Role = role;
        }

        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}