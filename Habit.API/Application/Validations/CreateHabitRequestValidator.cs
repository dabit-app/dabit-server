using FluentValidation;
using Habit.API.Application.DTO.Requests;
using Habit.API.Application.Validations.Shared;

namespace Habit.API.Application.Validations
{
    public class CreateHabitRequestValidator : AbstractValidator<CreateHabitRequest>
    {
        public CreateHabitRequestValidator() {
            RuleFor(o => o.Name).SetValidator(new HabitNameValidator());
        }
    }
}