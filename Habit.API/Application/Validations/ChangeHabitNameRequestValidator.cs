using Application.Application.DTO.Requests;
using Application.Application.Validations.Shared;
using FluentValidation;

namespace Application.Application.Validations
{
    public class ChangeHabitNameRequestValidator : AbstractValidator<ChangeHabitNameRequest>
    {
        public ChangeHabitNameRequestValidator() {
            RuleFor(o => o.Name).SetValidator(new HabitNameValidator());
        }
    }
}