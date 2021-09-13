using System;
using System.Text.Json.Serialization;
using Domain.Habits.Schedules;

namespace Domain.Habits.Events
{
    public class HabitEventUncompleted
    {
        public int NthEvent { get; }

        public HabitEventUncompleted(DateTime day, Schedule schedule) {
            NthEvent = schedule.GetNthEventAt(day);
        }
        
        [JsonConstructor]
        public HabitEventUncompleted(int nthEvent) {
            NthEvent = nthEvent;
        }
    }
}