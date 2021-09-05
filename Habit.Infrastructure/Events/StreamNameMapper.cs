using System;
using System.Reflection;

namespace Infrastructure.Events
{
    public static class StreamNameMapper
    {
        public static string ToStreamId<TStream>(object aggregateId) =>
            ToStreamId(typeof(TStream), aggregateId);

        private static string ToStreamId(MemberInfo streamType, object aggregateId) {
            return $"{streamType.Name}-{aggregateId}";
        }

        public static Guid RetrievedIdFromStreamName(Type type, string streamName) {
            var length = type.Name.Length + 1;
            if (Guid.TryParse(streamName[length..], out var guid))
                return guid;
            throw new Exception("Failed to find the Guid from the stream name");
        }
    }
}