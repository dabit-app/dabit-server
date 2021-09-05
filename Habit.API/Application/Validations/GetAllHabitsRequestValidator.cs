using Application.Application.DTO.Requests;
using FluentValidation;

namespace Application.Application.Validations
{
    public class GetAllHabitsRequestValidator : AbstractValidator<GetAllHabitsRequest>
    {
        public GetAllHabitsRequestValidator() {
            RuleFor(request => request.Page).GreaterThan(0);
        }
    }
}