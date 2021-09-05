using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using EventStore.Client;
using Infrastructure.Events;
using Infrastructure.Exceptions;
using Infrastructure.Serialization;

namespace Infrastructure.Extensions
{
    public static class AggregateStreamExtensions
    {
        public static async Task<T?> AggregateStream<T>(
            this EventStoreClient eventStoreClient,
            Guid id,
            CancellationToken cancellationToken
        ) where T : class, IProjection {
            var readResult = eventStoreClient.ReadStreamAsync(
                Direction.Forwards,
                StreamNameMapper.ToStreamId<T>(id),
                StreamPosition.Start,
                cancellationToken: cancellationToken
            );

            var readState = await readResult.ReadState;

            if (readState == ReadState.StreamNotFound)
                throw AggregateNotFoundException.For<T>(id);

            var aggregate = (T) Activator.CreateInstance(typeof(T), true)!;

            await foreach (var @event in readResult)
            {
                var eventData = @event.Deserialize();
                aggregate.When(eventData);
            }

            return aggregate;
        }
    }
}