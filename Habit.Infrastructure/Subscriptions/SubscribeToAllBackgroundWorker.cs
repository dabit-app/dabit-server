using System;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Infrastructure.Events;
using Infrastructure.Serialization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Infrastructure.Subscriptions
{
    public class SubscribeToAllBackgroundWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscribeToAllBackgroundWorker> _logger;
        private readonly EventStoreClient _eventStoreClient;
        private readonly IAggregateMapper _aggregateMapper;
        private readonly ISubscriptionCheckpointRepository _checkpointRepository;
        private CancellationTokenSource? _cts;

        public SubscribeToAllBackgroundWorker(
            IServiceProvider serviceProvider,
            ILogger<SubscribeToAllBackgroundWorker> logger,
            EventStoreClient eventStoreClient,
            IAggregateMapper aggregateMapper,
            ISubscriptionCheckpointRepository checkpointRepository
        ) {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _eventStoreClient = eventStoreClient;
            _aggregateMapper = aggregateMapper;
            _checkpointRepository = checkpointRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            _logger.LogInformation("Event reader background worker - start");
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            await Resubscribe();
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            _cts?.Cancel();
            _logger.LogInformation("Event reader background worker - stopped");

            return Task.CompletedTask;
        }

        private async Task SubscribeToAll(CancellationToken cancellationToken) {
            _logger.LogInformation("Subscribing to dabit events ...");

            using var scope = _serviceProvider.CreateScope();

            var streamPosition = await _checkpointRepository.LoadPosition(cancellationToken);
            var position = streamPosition != null
                ? new Position(streamPosition.Value, streamPosition.Value)
                : Position.Start;

            await _eventStoreClient.SubscribeToAllAsync(
                cancellationToken: cancellationToken,
                start: position,
                eventAppeared: HandleEvent,
                subscriptionDropped: HandleDrop,
                filterOptions: new SubscriptionFilterOptions(EventTypeFilter.ExcludeSystemEvents())
            );

            _logger.LogInformation("Successfully subscribed");
            _logger.LogInformation("Stream taken from position {}", position.CommitPosition);
        }

        private async Task HandleEvent(
            StreamSubscription subscription,
            ResolvedEvent resolvedEvent,
            CancellationToken cancellationToken
        ) {
            try
            {
                if (resolvedEvent.Event.Data.IsEmpty)
                    return;

                using var scope = _serviceProvider.CreateScope();

                _logger.LogInformation($"Found {resolvedEvent.Event.EventType}");

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var streamName = resolvedEvent.Event.EventStreamId;
                var aggregateType = _aggregateMapper.StreamNameToAggregate(streamName);
                var @event = resolvedEvent.Deserialize();
                var id = StreamNameMapper.RetrievedIdFromStreamName(aggregateType, streamName);

                var aggregateEvent = Activator.CreateInstance(
                    typeof(AggregateEvent<>).MakeGenericType(@event.GetType()),
                    id, @event
                )!;

                await mediator.Publish(aggregateEvent, cancellationToken);
                _checkpointRepository.SavePosition(resolvedEvent, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(
                    "Error consuming message: {ExceptionMessage} {ExceptionStackTrace}",
                    e.Message,
                    e.StackTrace
                );
            }
        }

        private void HandleDrop(
            StreamSubscription streamSubscription,
            SubscriptionDroppedReason subscriptionDroppedReason,
            Exception? exception
        ) {
            _logger.LogWarning(exception, "Subscription dropped with reason: {reason}", subscriptionDroppedReason);
            Resubscribe().Wait();
        }

        private async Task Resubscribe() {
            var retry = Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(
                    _ => TimeSpan.FromSeconds(1),
                    (_, _) => _logger.LogWarning("Failed to resubscribe.")
                );

            await retry.ExecuteAsync(async () => await SubscribeToAll(_cts!.Token));
        }
    }
}