using Application.Application.DTO.Requests;
using Application.Application.Validations.Shared;
using FluentValidation;

namespace Application.Application.Validations
{
    public class CreateHabitRequestValidator : AbstractValidator<CreateHabitRequest>
    {
        public CreateHabitRequestValidator() {
            RuleFor(o => o.Name).SetValidator(new HabitNameValidator());
        }
    }
}