using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Domain.Habits.Schedules
{
    [Flags]
    public enum DaysOfWeek : byte
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

    public static class DaysOfWeekExtension
    {
        public static IEnumerable<DayOfWeek> GetAllDayOfWeek(this DaysOfWeek days) {
            var list = new List<DayOfWeek>();

            if ((days & DaysOfWeek.Monday) != 0)
                list.Add(DayOfWeek.Monday);

            if ((days & DaysOfWeek.Tuesday) != 0)
                list.Add(DayOfWeek.Tuesday);

            if ((days & DaysOfWeek.Wednesday) != 0)
                list.Add(DayOfWeek.Wednesday);

            if ((days & DaysOfWeek.Thursday) != 0)
                list.Add(DayOfWeek.Thursday);

            if ((days & DaysOfWeek.Friday) != 0)
                list.Add(DayOfWeek.Friday);

            if ((days & DaysOfWeek.Saturday) != 0)
                list.Add(DayOfWeek.Saturday);

            if ((days & DaysOfWeek.Sunday) != 0)
                list.Add(DayOfWeek.Sunday);

            return list;
        }

        /// <summary>
        /// Return the number of days enabled
        /// </summary>
        public static int Count(this DaysOfWeek daysOfWeek) {
            return BitOperations.PopCount((uint) daysOfWeek);
        }

        /// <summary>
        /// Get the nth days of the week, only counting the enabled days and starting the week on a specific day.
        /// </summary>
        /// <example>If a week start on wednesday and only friday/monday are enabled, monday should return 2.</example>
        public static int? GetNthFrom(this DaysOfWeek source, DayOfWeek weekStartOn, DayOfWeek target) {
            var nthIndex = source
                .GetAllDayOfWeek()
                .OrderBy(o =>
                {
                    var number = (int) o - (int) weekStartOn;
                    if (number < 0)
                        number += 7;
                    return number;
                })
                .ToList()
                .FindIndex(o => o == target);

            if (nthIndex == -1)
                return null;
            
            return nthIndex + 1; // 0 indexed
        }
    }
}