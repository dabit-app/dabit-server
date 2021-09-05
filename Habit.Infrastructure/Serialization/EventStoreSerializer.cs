using System.Text;
using System.Text.Json;
using EventStore.Client;
using Infrastructure.Events;

namespace Infrastructure.Serialization
{
    public static class EventStoreSerializer
    {
        public static T Deserialize<T>(this ResolvedEvent resolvedEvent) => (T) Deserialize(resolvedEvent);

        public static object Deserialize(this ResolvedEvent resolvedEvent) {
            var eventType = EventTypeMapper.ToType(resolvedEvent.Event.EventType);
            return JsonSerializer.Deserialize(
                Encoding.UTF8.GetString(resolvedEvent.Event.Data.Span),
                eventType
            )!;
        }

        public static EventData ToJsonEventData(this object @event) {
            return new(
                Uuid.NewUuid(),
                EventTypeMapper.ToName(@event.GetType()),
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event))
            );
        }
    }
}