using Domain.Habits.Schedules;
using Domain.SeedWork.Extensions;
using Habit.API.Application.DTO.Shared;

namespace Habit.API.Application.DTO.Responses
{
    public record ScheduleResponse
    {
        public string StartDate { get; init; }
        public string? EndDate { get; init; }
        public TimeSpanDto Cadency { get; init; }
        public TimeSpanDto Duration { get; init; }
        public DaysOfWeekDto? DaysOfWeek { get; init; } = new ();

        public static ScheduleResponse From(Schedule from) {
            return new()
            {
                StartDate = from.StartDate.ToShortDate(),
                EndDate = from.EndDate?.ToShortDate(),
                Cadency = TimeSpanDto.From(from.Cadency),
                Duration = TimeSpanDto.From(from.Duration),
                DaysOfWeek = DaysOfWeekDto.From(from.DaysOfWeek)
            };
        }
    }
}