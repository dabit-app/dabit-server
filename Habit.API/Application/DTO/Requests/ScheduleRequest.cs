using System;
using Domain.Habits.Schedules;
using Habit.API.Application.DTO.Shared;

namespace Habit.API.Application.DTO.Requests
{
    public record ScheduleRequest
    {
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public TimeSpanDto Cadency { get; init; }
        public TimeSpanDto Duration { get; init; }
        public DaysOfWeekDto? DaysOfWeek { get; init; } = new();

        public Schedule ToSchedule() {
            return new(
                StartDate,
                EndDate,
                Cadency.ToTimeSpan(),
                Duration.ToTimeSpan(),
                DaysOfWeek?.ToDaysOfWeek() ?? Domain.Habits.Schedules.DaysOfWeek.None
            );
        }
    }
}