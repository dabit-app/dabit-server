using System.Threading.Tasks;
using Application.Application.Commands;
using Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Application.Application.Notifiers
{
    public class ChangeHabitsNameNotifier : IHabitsHubNotifier<ChangeHabitNameCommand>
    {
        public async Task Handle(IHubContext<HabitsHub, IHabitsHub> hub, ChangeHabitNameCommand command) {
            await hub.Clients.All.HabitNameChanged(command.Id, command.Name);
        }
    }
}