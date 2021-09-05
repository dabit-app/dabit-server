using Application.Application.DTO.Requests;
using FluentValidation;

namespace Application.Application.Validations
{
    public class MarkHabitDayCompletenessValidator : AbstractValidator<MarkHabitDayCompleteness>
    {
        public MarkHabitDayCompletenessValidator() {
            RuleFor(o => o.Day).NotEmpty();
        }
    }
}