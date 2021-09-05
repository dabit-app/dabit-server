using Application.Application.DTO.Shared;

namespace Application.Application.DTO.Requests
{
    public record DefineHabitScheduleRequest
    {
        public ScheduleDto Schedule { get; init; }
    }
}