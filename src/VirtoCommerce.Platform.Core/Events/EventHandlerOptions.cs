namespace VirtoCommerce.Platform.Core.Events;

public class EventHandlerOptions
{
    // true: RegisterEventHandler<TEvent, THandler>() resolves the handler per event in its own DI scope, so it gets the lifetime it was registered with.
    // false: legacy behavior, one root-resolved instance for the process lifetime regardless of the registered lifetime.
    public bool ResolveHandlersPerInvocation { get; set; } = true;
}
