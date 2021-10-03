namespace Habit.API.Application.DTO.Requests
{
    public record CreateHabitRequest
    {
        public string Name { get; init; }
    }
}