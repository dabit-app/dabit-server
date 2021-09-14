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
    ///  - DaysOfWeek = (Optional) in case you want to precise which day of the week.
    /// 
    /// Example of scenario:
    ///  - Schedule for everyday. (Duration = 1day, Cadency = 1day)
    ///  - Schedule for once every three days. (Duration = 1day, Cadency = 3day)
    ///  - Schedule for every week on tuesday and wednesday. (Duration = 1 week, Cadency = 1 week, DaysOfWeek = {Tue, Wed})
    ///  - Schedule for once but whenever you want, per week. (Duration = 1 week, Cadency = 1 week)
    /// </summary>
    public class Schedule
    {
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }
        public TimeSpan Cadency { get; }
        public TimeSpan Duration { get; }
        public DaysOfWeek DaysOfWeek { get; }

        public Schedule(
            DateTime startDate,
            DateTime? endDate,
            TimeSpan cadency,
            TimeSpan duration,
            DaysOfWeek daysOfWeek = DaysOfWeek.None
        ) {
            if (endDate <= startDate)
                throw new DomainException(typeof(Schedule), "The end date cannot be smaller or equals to start date");

            if (duration > cadency)
                throw new DomainException(typeof(Schedule), "The duration cannot be higher than cadency");

            if (daysOfWeek != DaysOfWeek.None && cadency.ToDays % 7 != 0)
                throw new DomainException(typeof(Schedule), "Cadency must be in a number of week.");

            if (daysOfWeek != DaysOfWeek.None && duration.ToDays != 7)
                throw new DomainException(typeof(Schedule), "Duration must be exactly a week when using day of week.");

            StartDate = startDate.Date;
            EndDate = endDate?.Date;
            Cadency = cadency;
            Duration = duration;
            DaysOfWeek = daysOfWeek;
        }

        /// <summary>Return the nth event for a given day according to the schedule.</summary>
        /// <remarks>
        /// This can be reversed back to a date span (2 dates).
        /// 
        /// It is possible because a habit is a list of non-overlapping interval and those
        /// interval are predictable from a schedule definition and the schedule is immutable.
        /// </remarks>
        public int GetNthEventAt(DateTime day) {
            if (!(day >= StartDate && (EndDate == null || day < EndDate)))
                throw new DomainException(typeof(Schedule), "The date is out of bound from the schedule");

            var deltaDay = day - StartDate;
            var cadencyTime = (int) Math.Floor(deltaDay.TotalDays / Cadency.ToDays);

            var minDate = StartDate.AddDays(Cadency.ToDays * cadencyTime);
            var maxDate = minDate.AddDays(Duration.ToDays);

            if (!(day >= minDate && day < maxDate))
                throw new DomainException(
                    typeof(Schedule),
                    $"The date should have been between {minDate.ToShortDate()} and {maxDate.ToShortDate()} (exclusive) to be valid."
                );

            if (DaysOfWeek == DaysOfWeek.None)
                return cadencyTime + 1;

            var daysActivatedPerWeek = DaysOfWeek.Count();
            var nthDayOfTheWeek = DaysOfWeek.GetNthFrom(StartDate.DayOfWeek, day.DayOfWeek);

            if (nthDayOfTheWeek == null)
                throw new DomainException(
                    typeof(DaysOfWeek),
                    "That day of the week was not enabled according to the schedule"
                );

            return cadencyTime * daysActivatedPerWeek + nthDayOfTheWeek.Value;
        }
    }
}