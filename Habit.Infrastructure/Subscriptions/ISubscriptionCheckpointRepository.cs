using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;

namespace Infrastructure.Subscriptions
{
    public interface ISubscriptionCheckpointRepository
    {
        void SavePosition(ResolvedEvent resolvedEvent, CancellationToken cancellationToken);
        void SavePosition(ulong streamPosition, CancellationToken cancellationToken);
        Task<ulong?> LoadPosition(CancellationToken cancellationToken);
    }
}