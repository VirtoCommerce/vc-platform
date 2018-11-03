
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Events
{
    public interface IEventHandler<in T> : IHandler<T> where T : IEvent
    {
    }
}
