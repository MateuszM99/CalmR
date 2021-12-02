﻿namespace Infrastructure.Identity.Authentication
{
    public class SignUpRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int? PsychologistId { get; set; }
    }
}