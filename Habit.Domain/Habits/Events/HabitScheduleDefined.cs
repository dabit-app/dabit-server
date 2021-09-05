using Domain.Habits.Schedules;

namespace Domain.Habits.Events
{
    public class HabitScheduleDefined
    {
        public Schedule Schedule { get; }

        public HabitScheduleDefined(Schedule schedule) {
            Schedule = schedule;
        }
    }
}