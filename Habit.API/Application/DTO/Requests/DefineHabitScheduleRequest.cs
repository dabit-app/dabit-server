namespace Habit.API.Application.DTO.Requests
{
    public record DefineHabitScheduleRequest
    {
        public ScheduleRequest Schedule { get; init; }
    }
}