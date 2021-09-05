using System;
using System.Text.Json.Serialization;
using Domain.Habits.Schedules;

namespace Domain.Habits.Events
{
    public class HabitDayUncompleted
    {
        public int IntervalNumber { get; }

        public HabitDayUncompleted(DateTime day, Schedule schedule) {
            IntervalNumber = schedule.GetIntervalNumberForDay(day);
        }
        
        [JsonConstructor]
        public HabitDayUncompleted(int intervalNumber) {
            IntervalNumber = intervalNumber;
        }
    }
}