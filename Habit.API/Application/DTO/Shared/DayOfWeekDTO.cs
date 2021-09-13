using Domain.Habits.Schedules;

namespace Application.Application.DTO.Shared
{
    public record DayOfWeekDto
    {
        public bool Monday { get; init; }
        public bool Tuesday { get; init; }
        public bool Wednesday { get; init; }
        public bool Thursday { get; init; }
        public bool Friday { get; init; }
        public bool Saturday { get; init; }
        public bool Sunday { get; init; }

        public DaysOfWeek ToDayOfWeek() {
            return
                (Monday ? DaysOfWeek.Monday : DaysOfWeek.None) |
                (Tuesday ? DaysOfWeek.Tuesday : DaysOfWeek.None) |
                (Wednesday ? DaysOfWeek.Wednesday : DaysOfWeek.None) |
                (Thursday ? DaysOfWeek.Thursday : DaysOfWeek.None) |
                (Friday ? DaysOfWeek.Friday : DaysOfWeek.None) |
                (Saturday ? DaysOfWeek.Saturday : DaysOfWeek.None) |
                (Sunday ? DaysOfWeek.Sunday : DaysOfWeek.None);
        }

        public static DayOfWeekDto? From(DaysOfWeek from) {
            if (from == DaysOfWeek.None)
                return null;
            
            return new DayOfWeekDto
            {
                Monday = from.HasFlag(DaysOfWeek.Monday),
                Tuesday = from.HasFlag(DaysOfWeek.Tuesday),
                Wednesday = from.HasFlag(DaysOfWeek.Wednesday),
                Thursday = from.HasFlag(DaysOfWeek.Thursday),
                Friday = from.HasFlag(DaysOfWeek.Friday),
                Saturday = from.HasFlag(DaysOfWeek.Saturday),
                Sunday = from.HasFlag(DaysOfWeek.Sunday)
            };
        }
    }
}