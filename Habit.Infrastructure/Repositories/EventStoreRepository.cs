using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using EventStore.Client;
using Infrastructure.Events;
using Infrastructure.Extensions;
using Infrastructure.Serialization;

namespace Infrastructure.Repositories
{
    public class EventStoreRepository<T> : IEventStoreRepository<T> where T : class, IAggregate
    {
        private readonly EventStoreClient _eventStore;

        public EventStoreRepository(EventStoreClient eventStoreClient) {
            _eventStore = eventStoreClient;
        }

        public Task<T?> Find(Guid id, CancellationToken cancellationToken) {
            return _eventStore.AggregateStream<T>(id, cancellationToken);
        }

        public async Task Store(T aggregate, CancellationToken cancellationToken) {
            var events = aggregate.DequeueUncommittedEvents();
            var eventsToStore = events.Select(EventStoreSerializer.ToJsonEventData).ToArray();

            await _eventStore.AppendToStreamAsync(
                StreamNameMapper.ToStreamId<T>(aggregate.Id),
                StreamState.Any,
                eventsToStore,
                cancellationToken: cancellationToken
            );
        }
    }
}