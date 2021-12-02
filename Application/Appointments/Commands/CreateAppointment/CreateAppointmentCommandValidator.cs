using System;
using FluentValidation;

namespace Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.PsychologistId)
                .NotEmpty();
            
            RuleFor(x => x.AppointmentDate)
                .NotNull()
                .GreaterThanOrEqualTo(DateTime.Now);

            RuleFor(x => x.AppointmentDurationTime)
                .NotNull()
                .GreaterThan(0);
        }
    }
}