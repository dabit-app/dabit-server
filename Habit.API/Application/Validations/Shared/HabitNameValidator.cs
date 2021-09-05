using FluentValidation;

namespace Application.Application.Validations.Shared
{
    public class HabitNameValidator : AbstractValidator<string>
    {
        public HabitNameValidator() {
            RuleFor(name => name)
                .NotNull()
                .Length(1, 64);
        }
    }
}