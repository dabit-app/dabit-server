namespace Domain.SeedWork
{
    public interface IAggregate : IProjection, IDeletable
    {
        int Version { get; }
        bool IsDeleted { get; }
        object[] DequeueUncommittedEvents();
    }
}