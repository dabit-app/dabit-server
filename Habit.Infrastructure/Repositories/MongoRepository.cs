using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : IIdentifiable
    {
        public IMongoCollection<T> Collection { get; }

        public MongoRepository(IMongoCollection<T> collection) {
            Collection = collection;
        }

        public async Task<T?> Find(Guid id, CancellationToken cancellationToken) {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task Add(T view, CancellationToken cancellationToken) {
            await Collection.InsertOneAsync(view, null, cancellationToken);
        }

        public async Task Update(Guid id, T view, CancellationToken cancellationToken) {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await Collection.ReplaceOneAsync(filter, view, cancellationToken: cancellationToken);
        }

        public async Task Upsert(Guid id, T view, CancellationToken cancellationToken) {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await Collection.ReplaceOneAsync(filter, view, new ReplaceOptions {IsUpsert = true}, cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken) {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await Collection.DeleteOneAsync(filter, cancellationToken);
        }
    }
}