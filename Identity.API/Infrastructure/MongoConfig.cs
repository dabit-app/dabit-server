using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Identity.API.Infrastructure
{
    public static class MongoConfig
    {
        public static void AddMongoDbContext(this IServiceCollection services, IConfiguration configuration) {
            var connectionUri = configuration.GetConnectionString("dabit-user-mongo-db");

            var client = new MongoClient(connectionUri);
            var database = client.GetDatabase("dabit-user");

            services.AddSingleton(database);
        }

        public static void AddMongoCollection<TModel>(
            this IServiceCollection services,
            string name,
            Action<IMongoCollection<TModel>>? options = null
        ) {
            services.AddScoped<IMongoCollection<TModel>>(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var collection = db.GetCollection<TModel>(name);
                options?.Invoke(collection);
                return collection;
            });
        }

        public static async Task AddHashIndex<TDocument>(
            this IMongoCollection<TDocument> collection,
            Expression<Func<TDocument, object>> field
        ) {
            var indexDefinition = Builders<TDocument>.IndexKeys.Hashed(field);
            var indexModel = new CreateIndexModel<TDocument>(indexDefinition);
            await collection.Indexes.CreateOneAsync(indexModel);
        }
    }
}