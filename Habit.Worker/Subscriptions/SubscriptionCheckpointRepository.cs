using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Habit.Worker.Subscriptions
{
    public class SubscriptionCheckpointRepository : MongoRepository<SubscriptionCheckpoint>,
        ISubscriptionCheckpointRepository
    {
        private static readonly Guid Id = Guid.Empty;

        public SubscriptionCheckpointRepository(IMongoCollection<SubscriptionCheckpoint> collection)
            : base(collection) {
        }

        public void SavePosition(ResolvedEvent resolvedEvent, CancellationToken cancellationToken) {
            SavePosition(resolvedEvent.Event.Position.CommitPosition, cancellationToken);
        }

        public async void SavePosition(ulong streamPosition, CancellationToken cancellationToken) {
            var checkpoint = new SubscriptionCheckpoint(Id, streamPosition);
            await Upsert(checkpoint.Id, checkpoint, cancellationToken);
        }

        public async Task<ulong?> LoadPosition(CancellationToken cancellationToken) {
            var checkpoint = await Find(Id, cancellationToken);
            return checkpoint?.Position;
        }
    }

    public static class SubscriptionCheckpointConfig
    {
        public static void AddCheckpointRepository(this IServiceCollection services, string name) {
            services.AddSingleton<IMongoCollection<SubscriptionCheckpoint>>(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                return db.GetCollection<SubscriptionCheckpoint>(name);
            });

            services.AddSingleton<ISubscriptionCheckpointRepository, SubscriptionCheckpointRepository>();
        }
    }
}