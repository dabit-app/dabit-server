namespace Habit.API.Application.DTO.Requests
{
    public record ChangeHabitNameRequest
    {
        public string Name { get; init; }
    }
}