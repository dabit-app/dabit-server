using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Habit.API.Application.Notifiers
{
    public interface INotifier<THub, THubDef, in TCommand> where THub : Hub<THubDef> where THubDef : class
    {
        Task Handle(IHubContext<THub, THubDef> hub, TCommand command);
    }
}