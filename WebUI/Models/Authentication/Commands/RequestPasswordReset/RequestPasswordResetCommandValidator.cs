using FluentValidation;
using Infrastructure.Identity.Authentication;

namespace CalmR.Models.Authentication.Commands.RequestPasswordReset
{
    public class RequestPasswordResetCommandValidator : AbstractValidator<ResetPasswordRequest>
    {
        public RequestPasswordResetCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}