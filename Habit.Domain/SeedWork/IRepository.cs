using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.SeedWork
{
    public interface IRepository<T> where T : IIdentifiable
    {
        public Task<T?> Find(Guid id, CancellationToken cancellationToken);
    }

    public interface IEventStoreRepository<T> : IRepository<T> where T : IAggregate
    {
        public Task Store(T aggregate, CancellationToken cancellationToken);
    }

    public interface INoSqlRepository<T> : IRepository<T> where T : IIdentifiable
    {
        public Task Add(T view, CancellationToken cancellationToken);

        public Task Update(Guid id, T view, CancellationToken cancellationToken);
        
        public Task Delete(Guid id, CancellationToken cancellationToken);
    }
}