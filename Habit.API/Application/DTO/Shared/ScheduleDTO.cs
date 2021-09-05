using System;
using Domain.Habits.Schedules;

namespace Application.Application.DTO.Shared
{
    public record ScheduleDto
    {
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public TimeSpanDto Cadency { get; init; }
        public TimeSpanDto Duration { get; init; }
        public DayOfWeekDto? DayOfWeek { get; init; } = new ();

        public Schedule ToSchedule() {
            return new(
                StartDate,
                EndDate,
                Cadency.ToTimeSpan(),
                Duration.ToTimeSpan(),
                DayOfWeek!.ToDayOfWeek()
            );
        }

        public static ScheduleDto From(Schedule from) {
            return new()
            {
                StartDate = from.StartDate,
                EndDate = from.EndDate,
                Cadency = TimeSpanDto.From(from.Cadency),
                Duration = TimeSpanDto.From(from.Duration),
                DayOfWeek = DayOfWeekDto.From(from.DayOfWeek)
            };
        }
    }
}