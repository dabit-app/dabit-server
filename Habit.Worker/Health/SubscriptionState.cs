using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Habit.Worker.Health
{
    public class SubscriptionState
    {
        public HealthStatus State { get; set; }
    }
}