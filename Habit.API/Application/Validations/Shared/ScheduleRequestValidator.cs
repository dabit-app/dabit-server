using System;
using FluentValidation;
using Habit.API.Application.DTO.Requests;

namespace Habit.API.Application.Validations.Shared
{
    public class ScheduleRequestValidator : AbstractValidator<ScheduleRequest>
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