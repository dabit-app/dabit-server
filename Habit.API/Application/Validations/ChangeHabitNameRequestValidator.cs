using FluentValidation;
using Habit.API.Application.DTO.Requests;
using Habit.API.Application.Validations.Shared;

namespace Habit.API.Application.Validations
{
    public class ChangeHabitNameRequestValidator : AbstractValidator<ChangeHabitNameRequest>
    {
        public ChangeHabitNameRequestValidator() {
            RuleFor(o => o.Name).SetValidator(new HabitNameValidator());
        }
    }
}