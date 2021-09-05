using System;

namespace Domain.Habits.Schedules
{
    
    // note: System.DayOfWeek is a single value enum
    //       this DayOfWeek allow multiple choice
    
    [Flags]
    public enum DayOfWeek : byte
    {
        None = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64
    }
    
    public static class DayOfWeekExtension
    {
        public static DayOfWeek ToDayOfWeek(this DateTime datetime) {
            return datetime.DayOfWeek switch
            {
                System.DayOfWeek.Sunday => DayOfWeek.Sunday,
                System.DayOfWeek.Monday => DayOfWeek.Monday,
                System.DayOfWeek.Tuesday => DayOfWeek.Tuesday,
                System.DayOfWeek.Wednesday => DayOfWeek.Wednesday,
                System.DayOfWeek.Thursday => DayOfWeek.Thursday,
                System.DayOfWeek.Friday => DayOfWeek.Friday,
                System.DayOfWeek.Saturday => DayOfWeek.Saturday,
                _ => throw new ArgumentOutOfRangeException(nameof(datetime))
            };
        }

        public static int ToNumber(this System.DayOfWeek day) {
            return day switch
            {
                System.DayOfWeek.Sunday => 1,
                System.DayOfWeek.Monday => 2,
                System.DayOfWeek.Tuesday => 3,
                System.DayOfWeek.Wednesday => 4,
                System.DayOfWeek.Thursday => 5,
                System.DayOfWeek.Friday => 6,
                System.DayOfWeek.Saturday => 7,
                _ => throw new ArgumentOutOfRangeException(nameof(day))
            };
        }
    }
}