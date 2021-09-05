using System;
using System.Text;
using Application.Extensions;
using Domain.SeedWork.Exceptions;

namespace Domain.Habits.Schedules
{
    /// <summary>
    /// A schedule represent how event should be placed on a calendar.
    ///
    ///  - Duration = the time in which you can perform an habit.
    ///  - Cadency = the timespan in which it loops over.
    ///  - DayOfWeek = (Optional) in case you want to precise which day of the week.
    /// 
    /// Example of scenario:
    ///  - Schedule for everyday. (Duration = 1day, Cadency = 1day)
    ///  - Schedule for once every three days. (Duration = 1day, Cadency = 3day)
    ///  - Schedule for every week on tuesday and wednesday. (Duration = 1 week, Cadency = 1 week, DayOfWeek = {Tue, Wed})
    ///  - Schedule for once but whenever you want, per week. (Duration = 1 week, Cadency = 1 week)
    /// </summary>
    public class Schedule
    {
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }
        public TimeSpan Cadency { get; }
        public TimeSpan Duration { get; }
        public DayOfWeek DayOfWeek { get; }

        public Schedule(
            DateTime startDate,
            DateTime? endDate,
            TimeSpan cadency,
            TimeSpan duration,
            DayOfWeek dayOfWeek = DayOfWeek.None
        ) {
            if (endDate <= startDate)
                throw new DomainException(typeof(Schedule), "The end date cannot be smaller or equals to start date");

            if (duration > cadency)
                throw new DomainException(typeof(Schedule), "The duration cannot be higher than cadency");

            if (dayOfWeek != DayOfWeek.None && cadency.ToDays % 7 != 0)
                throw new DomainException(typeof(Schedule), "Cadency must be in a number of week.");

            if (dayOfWeek != DayOfWeek.None && duration.ToDays != 7)
                throw new DomainException(typeof(Schedule), "Duration must be exactly a week when using day of week.");

            StartDate = startDate.Date;
            EndDate = endDate?.Date;
            Cadency = cadency;
            Duration = duration;
            DayOfWeek = dayOfWeek;
        }

        /// <summary>
        /// Validate if a day can be marked as completed
        /// </summary>
        public (bool, string) ValidateDayCanBeCompleted(DateTime day) {
            if (!(day >= StartDate && (EndDate == null || day < EndDate)))
                return (false, "date is out of bound from the schedule");

            if (DayOfWeek != DayOfWeek.None && !DayOfWeek.HasFlag(day.ToDayOfWeek()))
            {
                return (
                    false,
                    $"date is on {day.ToDayOfWeek().ToString()} but this is not accepted according to the schedule"
                );
            }


            var deltaDay = day - StartDate;
            var cadencyTime = (int) Math.Floor(deltaDay.TotalDays / Cadency.ToDays);

            var minDate = StartDate.AddDays(Cadency.ToDays * cadencyTime);
            var maxDate = minDate.AddDays(Duration.ToDays);

            return (
                day >= minDate && day < maxDate,
                $"date should have been between {minDate.ToShortDate()} and {maxDate.ToShortDate()} (exclusive)"
            );
        }

        /// <summary>
        /// Return an interval number which represent an unique interval according to the schedule.
        /// This can be reversed back to a timespan (2 date).
        ///
        /// It is possible because a habit is a list of non-overlapping interval and those
        /// interval are predictable from a schedule definition and the schedule is immutable.
        /// </summary>
        public int GetIntervalNumberForDay(DateTime day) {
            var cadencyDays = Cadency.ToDays;
            var deltaDay = day - StartDate;
            var cadencyTime = (int) Math.Floor(deltaDay.TotalDays / cadencyDays);

            var dayOfWeekShift = 0;
            if (DayOfWeek != DayOfWeek.None)
                dayOfWeekShift = day.DayOfWeek.ToNumber();

            return cadencyTime * cadencyDays + dayOfWeekShift;
        }
    }
}