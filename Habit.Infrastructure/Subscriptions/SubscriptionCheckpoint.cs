using System;
using Domain.SeedWork;

namespace Infrastructure.Subscriptions
{
    public record SubscriptionCheckpoint(Guid Id, ulong Position) : IIdentifiable;
}