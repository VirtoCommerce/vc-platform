using System;

namespace VirtoCommerce.Platform.Core.Events;

// Implemented by the handlers that RegisterEventHandler<TEvent, THandler>() subscribes: stand-ins that resolve THandler
// from a DI scope of their own for every event. A registrar sees an ordinary handler; through this interface it also
// sees the type the handler stands in for.
public interface IScopedEventHandler
{
    // The THandler the module registered.
    Type HandlerType { get; }

    // The runtime type THandler resolves to, which differs from HandlerType when a derived type is registered in DI for it.
    // Resolves the handler once.
    Type ResolveImplementationType();
}
