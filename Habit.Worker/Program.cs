using Infrastructure;
using Infrastructure.Subscriptions;
using MediatR;
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
                .ConfigureServices((buildContext, services) =>
                {
                    // application
                    services.AddMediatR(typeof(Program));
                    
                    // infrastructure
                    services.AddEventStoreDbContext(buildContext.Configuration);
                    services.AddMongoDbContext(buildContext.Configuration);
                    services.AddInfrastructureDependenciesInjection();
                    
                    // background worker
                    services.AddHostedService<SubscribeToAllBackgroundWorker>();
                });
        }
    }
}