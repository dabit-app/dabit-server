using System;

namespace Infrastructure.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public Guid Id { get; }

        private AggregateNotFoundException(string typeName, Guid id)
            : base($"{typeName} with id '{id}' was not found") {
            Id = id;
        }

        public static AggregateNotFoundException For<T>(Guid id) {
            return new(typeof(T).Name, id);
        }
    }
}