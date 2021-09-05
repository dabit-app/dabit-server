using System;

namespace Domain.SeedWork
{
    public interface IIdentifiable
    {
        public Guid Id { get; }
    }
}