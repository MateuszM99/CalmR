using FluentValidation;

namespace CalmR.Models.Authenticate.Command
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(x => x.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}