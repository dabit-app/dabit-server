using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using Infrastructure.Exceptions;

namespace Infrastructure.Extensions
{
    public static class EventStoreRepositoryExtensions
    {
        public static async Task<T> Get<T>(
            this IRepository<T> repository,
            Guid id,
            Func<T, bool>? isOwner = null,
            CancellationToken cancellationToken = default
        ) where T : IIdentifiable {
            var entity = await repository.Find(id, cancellationToken);
            
            if (entity == null) 
                throw AggregateNotFoundException.For<T>(id);

            if (!(isOwner?.Invoke(entity) ?? true))
                throw new AggregateNotOwnedException();

            return entity;
        }

        public static async Task GetAndUpdate<T>(
            this IEventStoreRepository<T> repository,
            Guid id,
            Action<T> modification,
            Func<T, bool>? isOwner = null,
            CancellationToken cancellationToken = default
        ) where T : IAggregate {
            var entity = await repository.Get(id, isOwner, cancellationToken);
            modification(entity);
            await repository.Store(entity, cancellationToken);
        }
    }
}