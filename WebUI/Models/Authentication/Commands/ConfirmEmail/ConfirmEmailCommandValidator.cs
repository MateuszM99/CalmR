using FluentValidation;
using Infrastructure.Identity.Authentication;

namespace CalmR.Models.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailModel>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Code)
                .NotEmpty();
        }
    }
}