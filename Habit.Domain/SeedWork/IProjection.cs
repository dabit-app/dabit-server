namespace Domain.SeedWork
{
    public interface IProjection : IIdentifiable
    {
        void When(object @event);
    }
}