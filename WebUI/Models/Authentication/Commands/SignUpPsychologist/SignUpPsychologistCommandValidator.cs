using Application.Common.Extensions;
using FluentValidation;

namespace CalmR.Models.Authentication.Commands.SignUpPsychologist
{
    public class SignUpPsychologistCommandValidator : AbstractValidator<SignUpPsychologistCommand>
    {
        SignUpPsychologistCommandValidator()
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