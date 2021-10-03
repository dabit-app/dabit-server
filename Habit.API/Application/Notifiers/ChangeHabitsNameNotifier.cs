using System.Threading.Tasks;
using Habit.API.Application.Commands;
using Habit.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Habit.API.Application.Notifiers
{
    public class ChangeHabitsNameNotifier : IHabitsHubNotifier<ChangeHabitNameCommand>
    {
        public async Task Handle(IHubContext<HabitsHub, IHabitsHub> hub, ChangeHabitNameCommand command) {
            await hub.Clients.All.HabitNameChanged(command.Id, command.Name);
        }
    }
}