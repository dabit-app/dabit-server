using System;

namespace Habit.API.Application.DTO.Requests
{
    public record MarkHabitDayCompleteness
    {
        public DateTime Day { get; init; }
    }
}