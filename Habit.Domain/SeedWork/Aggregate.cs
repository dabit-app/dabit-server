using System;
using System.Collections.Generic;

namespace Domain.SeedWork
{
    public abstract class Aggregate : IAggregate
    {
        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        public bool IsDeleted { get; protected set; }

        private readonly Queue<object> _uncommitedEvents = new();

        public virtual void When(object @event) {
        }

        public object[] DequeueUncommittedEvents() {
            var dequeuedEvents = _uncommitedEvents.ToArray();
            _uncommitedEvents.Clear();
            return dequeuedEvents;
        }

        public void Enqueue(object @event) {
            _uncommitedEvents.Enqueue(@event);
        }
    }
}