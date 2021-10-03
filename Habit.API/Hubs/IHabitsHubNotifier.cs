using Habit.API.Application.Notifiers;

namespace Habit.API.Hubs
{
    public interface IHabitsHubNotifier<in TCommand> : INotifier<HabitsHub, IHabitsHub, TCommand>
    {
    }
}