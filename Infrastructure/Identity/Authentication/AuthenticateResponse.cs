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
            UserName = user.UserName;
            ProfileImageUrl = user.Psychologist?.ProfileImageUrl;
            Token = token;
            Role = role;
            PsychologistId = user.PsychologistId;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public int? PsychologistId { get; set; }
    }
}