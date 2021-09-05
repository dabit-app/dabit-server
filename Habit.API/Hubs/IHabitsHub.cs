using System;
using System.Threading.Tasks;

namespace Application.Hubs
{
    public interface IHabitsHub
    {
        Task HabitNameChanged(Guid habitId, string name);
    }
}