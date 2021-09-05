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

        public DayOfWeek ToDayOfWeek() {
            return
                (Monday ? DayOfWeek.Monday : DayOfWeek.None) |
                (Tuesday ? DayOfWeek.Tuesday : DayOfWeek.None) |
                (Wednesday ? DayOfWeek.Wednesday : DayOfWeek.None) |
                (Thursday ? DayOfWeek.Thursday : DayOfWeek.None) |
                (Friday ? DayOfWeek.Friday : DayOfWeek.None) |
                (Saturday ? DayOfWeek.Saturday : DayOfWeek.None) |
                (Sunday ? DayOfWeek.Sunday : DayOfWeek.None);
        }

        public static DayOfWeekDto? From(DayOfWeek from) {
            if (from == DayOfWeek.None)
                return null;
            
            return new DayOfWeekDto
            {
                Monday = from.HasFlag(DayOfWeek.Monday),
                Tuesday = from.HasFlag(DayOfWeek.Tuesday),
                Wednesday = from.HasFlag(DayOfWeek.Wednesday),
                Thursday = from.HasFlag(DayOfWeek.Thursday),
                Friday = from.HasFlag(DayOfWeek.Friday),
                Saturday = from.HasFlag(DayOfWeek.Saturday),
                Sunday = from.HasFlag(DayOfWeek.Sunday)
            };
        }
    }
}