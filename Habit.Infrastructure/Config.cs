using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.SeedWork;
using EventStore.Client;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infrastructure
{
    public static class Config
    {
        public static void AddEventStoreDbContext(this IServiceCollection services, IConfiguration configuration) {
            var connectionUri = configuration.GetConnectionString("dabit-event-store-db");
            services.AddSingleton(new EventStoreClient(EventStoreClientSettings.Create(connectionUri)));
        }

        public static void AddMongoDbContext(this IServiceCollection services, IConfiguration configuration) {
            var connectionUri = configuration.GetConnectionString("dabit-mongo-db");

            var client = new MongoClient(connectionUri);
            var database = client.GetDatabase("dabit");

            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));
            
            services.AddSingleton(database);
        }

        public static void AddMongoRepository<TView>(
            this IServiceCollection services,
            string name,
            Action<IMongoCollection<TView>>? options = null
        )
            where TView : IIdentifiable {
            services.AddScoped<IMongoCollection<TView>>(provider =>
            {
                var db = provider.GetRequiredService<IMongoDatabase>();
                var collection = db.GetCollection<TView>(name);
                options?.Invoke(collection);
                return collection;
            });

            services.AddScoped<IMongoRepository<TView>, MongoRepository<TView>>();
        }

        public static async Task AddGuidIndex<TDocument>(
            this IMongoCollection<TDocument> collection,
            Expression<Func<TDocument, object>> field
        ) {
            var indexDefinition = Builders<TDocument>.IndexKeys.Ascending(field);
            var indexModel = new CreateIndexModel<TDocument>(indexDefinition);
            await collection.Indexes.CreateOneAsync(indexModel);
        }
    }
}