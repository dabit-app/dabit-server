using Domain.Habits.Schedules;
using FluentValidation;
using Habit.API.Application.DTO.Shared;

namespace Habit.API.Application.Validations.Shared
{
    public class TimeSpanRequestValidator : AbstractValidator<TimeSpanDto>
    {
        public TimeSpanRequestValidator() {
            RuleFor(o => o.Count)
                .NotNull()
                .GreaterThan(0);

            RuleFor(o => o.Unit)
                .NotEmpty()
                .IsEnumName(typeof(TimeUnit), false);
        }
    }
}