using Habit.Worker.Health;
using Habit.Worker.Subscriptions;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Habit.Worker
{
    public static class Program
    {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(Configure);
        }

        private static void Configure(HostBuilderContext buildContext, IServiceCollection services) {
            // application
            services.AddMediatR(typeof(Program));

            // infrastructure
            services.AddEventStoreDbContext(buildContext.Configuration);
            services.AddMongoDbContext(buildContext.Configuration);
            services.AddCheckpointRepository("subscription-checkpoint");
            services.AddInfrastructureDependenciesInjection();

            // singleton
            services.AddSingleton<SubscriptionState>();
            services.AddSingleton<SubscribeToAllBackgroundWorker>();

            // health checks
            services.AddHealthChecks()
                .AddMongoDb(buildContext.Configuration.GetConnectionString("dabit-mongo-db"))
                .AddSubscriptionCheck();
            services.ConfigureHealthCheckPublisher();

            // background worker
            services.AddHostedService(provider => provider.GetRequiredService<SubscribeToAllBackgroundWorker>());
        }
    }
}