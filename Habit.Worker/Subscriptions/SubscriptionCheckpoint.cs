using System;
using Domain.SeedWork;

namespace Habit.Worker.Subscriptions
{
    public record SubscriptionCheckpoint(Guid Id, ulong Position) : IIdentifiable;
}