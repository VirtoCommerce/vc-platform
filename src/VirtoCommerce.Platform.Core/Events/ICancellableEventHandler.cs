using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Events
{
    public interface ICancellableEventHandler<in T> : ICancellableHandler<T> where T : IEvent
    {
    }
}
