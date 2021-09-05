using Application.Application.Notifiers;

namespace Application.Hubs
{
    public interface IHabitsHubNotifier<in TCommand> : INotifier<HabitsHub, IHabitsHub, TCommand>
    {
    }
}