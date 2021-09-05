using System;
using Domain.SeedWork;
using MediatR;

namespace Infrastructure.Events
{
    public class AggregateEvent<TEvent> : IAggregateEvent<TEvent>
    {
        public Guid Id { get; }
        public TEvent Event { get; }

        public AggregateEvent(Guid id, TEvent @event) {
            Id = id;
            Event = @event;
        }
    }

    public interface IAggregateEvent<out TEvent> : IIdentifiable, INotification
    {
        public TEvent Event { get; }
    }
}