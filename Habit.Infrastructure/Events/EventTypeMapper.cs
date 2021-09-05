using System;
using System.Collections.Concurrent;

namespace Infrastructure.Events
{
    public class EventTypeMapper
    {
        private static readonly EventTypeMapper Instance = new();

        private readonly ConcurrentDictionary<Type, string> _typeNameMap = new();
        private readonly ConcurrentDictionary<string, Type> _typeMap = new();

        public static string ToName(Type eventType) => Instance._typeNameMap.GetOrAdd(eventType, _ =>
        {
            var eventTypeName = eventType.FullName!.Replace(".", "_");

            Instance._typeMap.AddOrUpdate(eventTypeName, eventType, (_, _) => eventType);

            return eventTypeName;
        });

        public static Type ToType(string eventTypeName) => Instance._typeMap.GetOrAdd(eventTypeName, _ =>
        {
            var type = TypeProvider.GetFirstMatchingTypeFromCurrentDomainAssembly(eventTypeName.Replace("_", "."))!;

            if (type == null)
                throw new Exception($"Type map for '{eventTypeName}' wasn't found!");

            Instance._typeNameMap.AddOrUpdate(type, eventTypeName, (_, _) => eventTypeName);

            return type;
        });
    }
}