using Application.Common.Extensions;
using FluentValidation;
using Infrastructure.Identity.Authentication;

namespace CalmR.Models.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Code)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Password(9);
        }
    }
}