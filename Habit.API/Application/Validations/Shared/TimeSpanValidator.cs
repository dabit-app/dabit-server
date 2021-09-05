using Application.Application.DTO.Shared;
using Domain.Habits.Schedules;
using FluentValidation;

namespace Application.Application.Validations.Shared
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