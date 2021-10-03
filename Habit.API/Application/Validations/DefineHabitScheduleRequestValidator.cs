using FluentValidation;
using Habit.API.Application.DTO.Requests;
using Habit.API.Application.Validations.Shared;

namespace Habit.API.Application.Validations
{
    public class DefineHabitScheduleRequestValidator : AbstractValidator<DefineHabitScheduleRequest>
    {
        public DefineHabitScheduleRequestValidator() {
            RuleFor(o => o.Schedule).NotEmpty().SetValidator(new ScheduleRequestValidator());
        }
    }
}