using System;

namespace Application.Application.DTO.Requests
{
    public record MarkHabitDayCompleteness
    {
        public DateTime Day { get; init; }
    }
}