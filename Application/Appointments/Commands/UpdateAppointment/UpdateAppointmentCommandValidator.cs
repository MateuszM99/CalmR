using System;
using FluentValidation;

namespace Application.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentCommandValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .NotNull()
                .GreaterThanOrEqualTo(DateTime.Now);

            RuleFor(x => x.AppointmentDurationTime)
                .NotNull()
                .GreaterThan(0);
        }
    }
}