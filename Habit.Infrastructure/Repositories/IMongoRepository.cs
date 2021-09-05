using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public interface IMongoRepository<T> : INoSqlRepository<T> where T : IIdentifiable
    {
        public IMongoCollection<T> Collection { get; }
        
        public Task Upsert(Guid id, T view, CancellationToken cancellationToken);
    }
}