using FluentValidation;
using Habit.API.Application.DTO.Requests;

namespace Habit.API.Application.Validations
{
    public class GetAllHabitsRequestValidator : AbstractValidator<GetAllHabitsRequest>
    {
        public GetAllHabitsRequestValidator() {
            RuleFor(request => request.Page).GreaterThan(0);
        }
    }
}