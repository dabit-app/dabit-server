namespace Domain.Habits.Events
{
    public class HabitNameChanged
    {
        public string Name { get; }

        public HabitNameChanged(string name) {
            Name = name;
        }
    }
}