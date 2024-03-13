using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Events;

public interface IEventHandlerRegistrar
{
    void RegisterEventHandler<T>(Func<T, Task> handler) where T : IEvent;
    void RegisterEventHandler<T>(Func<T, CancellationToken, Task> handler) where T : IEvent;
}
