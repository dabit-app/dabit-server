using System;
using Domain.Habits.Schedules;
using TimeSpan = Domain.Habits.Schedules.TimeSpan;

namespace Habit.API.Application.DTO.Shared
{
    public record TimeSpanDto
    {
        public string Unit { get; init; }
        public int Count { get; init; }

        public TimeSpan ToTimeSpan() {
            var unit = Unit.ToLower() switch
            {
                "day" => TimeUnit.Day,
                "week" => TimeUnit.Week,
                _ => throw new ArgumentOutOfRangeException()
            };

            return new TimeSpan(unit, Count);
        }

        public static TimeSpanDto From(TimeSpan from) {
            return new()
            {
                Unit = from.Unit switch
                {
                    TimeUnit.Day => "day",
                    TimeUnit.Week => "week",
                    _ => throw new ArgumentOutOfRangeException()
                },
                Count = from.Count
            };
        }
    }
}