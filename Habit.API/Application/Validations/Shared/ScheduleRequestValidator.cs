using System;
using Application.Application.DTO.Shared;
using FluentValidation;

namespace Application.Application.Validations.Shared
{
    public class ScheduleRequestValidator : AbstractValidator<ScheduleDto>
    {
        public ScheduleRequestValidator() {
            RuleFor(o => o.StartDate).NotEmpty().Must(NotDefaultDate);
            RuleFor(o => o.EndDate).Must(NotDefaultDate); // can be null
            RuleFor(o => o.Cadency).NotEmpty().SetValidator(new TimeSpanRequestValidator());
            RuleFor(o => o.Duration).NotEmpty().SetValidator(new TimeSpanRequestValidator());
        }

        private static bool NotDefaultDate(DateTime date) {
            return !date.Equals(default);
        }

        private static bool NotDefaultDate(DateTime? date) {
            return !date.HasValue || NotDefaultDate(date.Value);
        }
    }
}