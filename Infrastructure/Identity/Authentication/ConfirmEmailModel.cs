﻿namespace Infrastructure.Identity.Authentication
{
    public class ConfirmEmailModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}