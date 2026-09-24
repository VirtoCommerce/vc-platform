using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Messages;

namespace VirtoCommerce.Platform.Core.Bus
{
    // HandlerType is the type the handler was registered as. ImplementationType is its runtime type,
    // which differs when a derived type is registered in DI for a base type; Unregister accepts either.
    internal sealed record EventHandlerRegistration(
        Type EventType,
        Type HandlerType,
        Type ImplementationType,
        Func<IMessage, CancellationToken, Task> Handler);
}
