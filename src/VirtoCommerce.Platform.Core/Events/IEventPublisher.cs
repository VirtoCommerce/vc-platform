using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Events
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent;
    }
}
