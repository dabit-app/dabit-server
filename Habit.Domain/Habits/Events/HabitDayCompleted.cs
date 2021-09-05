using System;
using System.Text.Json.Serialization;
using Domain.Habits.Schedules;

namespace Domain.Habits.Events
{
    public class HabitDayCompleted
    {
        public int IntervalNumber { get; }

        public HabitDayCompleted(DateTime day, Schedule schedule) {
            IntervalNumber = schedule.GetIntervalNumberForDay(day);
        }

        [JsonConstructor]
        public HabitDayCompleted(int intervalNumber) {
            IntervalNumber = intervalNumber;
        }
    }
}