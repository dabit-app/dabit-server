using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Habit.Worker.Health
{
    public class SubscriptionHealthCheck : IHealthCheck
    {
        private readonly SubscriptionState _subscriptionState;

        public SubscriptionHealthCheck(SubscriptionState subscriptionState) {
            _subscriptionState = subscriptionState;
        }
        
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new()
        ) {
            return Task.FromResult(new HealthCheckResult(_subscriptionState.State));
        }
    }

    public static class SubscriptionHealthCheckConfig
    {
        public static void AddSubscriptionCheck(this IHealthChecksBuilder builder) {
            builder.AddCheck<SubscriptionHealthCheck>("subscription");
        }
    }
}