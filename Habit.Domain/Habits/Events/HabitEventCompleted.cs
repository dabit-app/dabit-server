using System;
using System.Text.Json.Serialization;
using Domain.Habits.Schedules;

namespace Domain.Habits.Events
{
    public class HabitEventCompleted
    {
        public int NthEvent { get; }

        public HabitEventCompleted(DateTime day, Schedule schedule) {
            NthEvent = schedule.GetNthEventAt(day);
        }

        [JsonConstructor]
        public HabitEventCompleted(int nthEvent) {
            NthEvent = nthEvent;
        }
    }
}