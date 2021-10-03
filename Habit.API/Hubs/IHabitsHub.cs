using System;
using System.Threading.Tasks;

namespace Habit.API.Hubs
{
    public interface IHabitsHub
    {
        Task HabitNameChanged(Guid habitId, string name);
    }
}