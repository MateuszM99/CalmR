namespace Infrastructure.Identity.Authentication
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}