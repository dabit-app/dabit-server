using System;
using System.Threading;
using System.Threading.Tasks;
using Habit.API.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Habit.API.Application.Behaviors
{
    public class HabitNotifyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IServiceProvider _serviceProvider;

        public HabitNotifyBehavior(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        ) {
            var response = await next();

            var hubContext = _serviceProvider.GetService<IHubContext<HabitsHub, IHabitsHub>>();
            var notifier = _serviceProvider.GetService<IHabitsHubNotifier<TRequest>>();
            
            if (hubContext != null && notifier != null)
                await notifier.Handle(hubContext, request);

            return response;
        }
    }
}