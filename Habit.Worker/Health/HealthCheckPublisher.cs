using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Habit.Worker.Health
{
    public class HealthCheckPublisher : IHealthCheckPublisher
    {
        private readonly string _filename;
        private HealthStatus _prevStatus = HealthStatus.Unhealthy;

        public HealthCheckPublisher(IConfiguration configuration) {
            _filename = configuration.GetValue("HealthChecks:FilePath", "/tmp/healthy");
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken) {
            var fileExists = _prevStatus == HealthStatus.Healthy;

            if (report.Status == HealthStatus.Healthy)
            {
                using var _ = File.Create(_filename);
            }
            else if (fileExists)
            {
                File.Delete(_filename);
            }

            _prevStatus = report.Status;

            return Task.CompletedTask;
        }
    }

    public static class HealthCheckPublisherConfig
    {
        public static void ConfigureHealthCheckPublisher(this IServiceCollection service) {
            service.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();
            service.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(5);
                options.Period = TimeSpan.FromSeconds(20);
            });
        }
    }
}