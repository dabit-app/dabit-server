using Application.Application.DTO.Requests;
using Application.Application.Validations.Shared;
using FluentValidation;

namespace Application.Application.Validations
{
    public class DefineHabitScheduleRequestValidator : AbstractValidator<DefineHabitScheduleRequest>
    {
        public DefineHabitScheduleRequestValidator() {
            RuleFor(o => o.Schedule).NotEmpty().SetValidator(new ScheduleRequestValidator());
        }
    }
}