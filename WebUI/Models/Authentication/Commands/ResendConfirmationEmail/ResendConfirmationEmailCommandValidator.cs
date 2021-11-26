using FluentValidation;

namespace CalmR.Models.Authentication.Commands.ResendConfirmationEmail
{
    public class ResendConfirmationEmailCommandValidator : AbstractValidator<string>
    {
        public ResendConfirmationEmailCommandValidator()
        {
            RuleFor(x => x)
                .NotEmpty();
        }
    }
}