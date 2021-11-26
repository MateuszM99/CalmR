using FluentValidation;

namespace Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .NotNull();

            RuleFor(x => x.AppointmentDurationTime)
                .NotNull();
        }
    }
}