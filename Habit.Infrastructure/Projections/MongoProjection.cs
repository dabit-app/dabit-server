using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using Infrastructure.Events;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Projections
{
    public class MongoProjection<TEvent, TView> : INotificationHandler<IAggregateEvent<TEvent>>
        where TView : IProjection, IIdentifiable, IDeletable
        where TEvent : notnull
    {
        private readonly ILogger<MongoProjection<TEvent, TView>> _logger;
        private readonly IMongoRepository<TView> _repository;

        public MongoProjection(
            ILogger<MongoProjection<TEvent, TView>> logger,
            IMongoRepository<TView> repository
        ) {
            _logger = logger;
            _repository = repository;
        }

        public async Task Handle(IAggregateEvent<TEvent> notification, CancellationToken ct) {
            var entity = await _repository.Find(notification.Id, ct)
                         ?? (TView) Activator.CreateInstance(typeof(TView), true)!;

            entity.When(notification.Event);

            if (entity.IsDeleted)
                await _repository.Delete(entity.Id, ct);
            else
                await _repository.Upsert(entity.Id, entity, ct);

            _logger.LogInformation(
                "Projection on mongodb done for "
                + $"view: {typeof(TView).Name}, "
                + $"event: {typeof(TEvent).Name}, "
                + $"id: {notification.Id}."
            );
        }
    }
}