using Domain.SeedWork;
using Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Projections
{
    public static class MongoProjectionConfig
    {
        public static void Project<TEvent, TView>(this IServiceCollection services)
            where TView : IProjection, IIdentifiable, IDeletable
            where TEvent : notnull {
            services.AddTransient<
                INotificationHandler<AggregateEvent<TEvent>>,
                MongoProjection<TEvent, TView>
            >();
        }
    }
}