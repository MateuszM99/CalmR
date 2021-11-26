using Application.Common.Extensions;
using FluentValidation;
using Infrastructure.Identity.Authentication;

namespace CalmR.Models.Authenticate.Commands.SignUp
{
    public class SignUpCommandValidator : AbstractValidator<SignUpRequest>
    {
        SignUpCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(6);
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .Password(9);
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password);

        }
    }
}