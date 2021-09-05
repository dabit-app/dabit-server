using System;
using System.Reflection;

namespace Domain.SeedWork.Exceptions
{
    /// <summary>
    /// Exception from a logic of the domain.
    /// It should be throw when something doesn't make sense.
    ///
    /// Eg. A TimeSpan object count cannot be equals or below 0 -> throw if given otherwise in the constructor.
    /// Eg. An aggregate was called to add an event, but this was not possible under the current state -> throw
    /// </summary>
    public class DomainException : Exception
    {
        public MemberInfo Context { get; }

        public DomainException(MemberInfo context, string message) : base(message) {
            Context = context;
        }
    }
}