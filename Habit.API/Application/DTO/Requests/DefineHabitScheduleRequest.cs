using Application.Application.DTO.Shared;

namespace Application.Application.DTO.Requests
{
    public record DefineHabitScheduleRequest
    {
        public ScheduleRequest Schedule { get; init; }
    }
}