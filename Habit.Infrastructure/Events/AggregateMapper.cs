using System;
using System.Collections.Generic;

namespace Infrastructure.Events
{
    public class AggregateMapper : IAggregateMapper
    {
        private readonly Dictionary<string, Type> _typeMap = new();

        public AggregateMapper(IEnumerable<Type> types) {
            foreach (var type in types)
                _typeMap.Add(type.Name, type);
        }

        /// <summary>
        /// Eg. From stream name (string)"Habit-318d1778-ba9e-4dc7-93b5-4fba72720966" -> (type)Habit
        /// </summary>
        public Type StreamNameToAggregate(string fullStreamName) {
            var typeName = fullStreamName[..fullStreamName.IndexOf('-')];
            return _typeMap[typeName];
        }
    }

    public interface IAggregateMapper
    {
        public Type StreamNameToAggregate(string fullStreamName);
    }
}