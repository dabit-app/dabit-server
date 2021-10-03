using FluentValidation;
using Habit.API.Application.DTO.Requests;

namespace Habit.API.Application.Validations
{
    public class MarkHabitDayCompletenessValidator : AbstractValidator<MarkHabitDayCompleteness>
    {
        public MarkHabitDayCompletenessValidator() {
            RuleFor(o => o.Day).NotEmpty();
        }
    }
}