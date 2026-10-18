
An event is something that has happened in the past. A domain event is, logically, something that happened in a particular domain, and something you want other parts of the same domain (in-process) to be aware of and potentially react to.

An important benefit of domain events is that side effects after something happened in a domain can be expressed explicitly instead of implicitly. Those side effects must be consistent so either all the operations related to the business task happen, or none of them. In addition, domain events enable a better separation of concerns among classes within the same domain.

## How to define domain events
A domain event is just a simple POCO type that represents an interesting occurrence in the domain.

```C#
public class CustomDomainEvent : DomainEvent
{
    public Customer Customer { get; set; }
}
```

## How to define a new event handler

```C#
public class CustomDomainEventHandler : IEventHandler<CustomDomainEvent>
{
    public async Task Handle(CustomDomainEvent message)
    {
        //Some logic here
    }
}
```

A handler that needs to observe cancellation implements `ICancellableEventHandler<CustomDomainEvent>` instead and receives the `CancellationToken` passed to `IEventPublisher.Publish`.

## How to register an event handler, subscribe to a domain event

Register the handler in the DI container with the lifetime it needs, then subscribe it to the event in `PostInitialize`:

```C#
public void Initialize(IServiceCollection serviceCollection)
{
    ...
    serviceCollection.AddTransient<CustomDomainEventHandler>();
    ...
}

public void PostInitialize(IApplicationBuilder appBuilder)
{
    ...
    appBuilder.RegisterEventHandler<CustomDomainEvent, CustomDomainEventHandler>();
    ...
}
```

Cancellable handlers are subscribed with `appBuilder.RegisterCancellableEventHandler<CustomDomainEvent, CustomDomainEventHandler>()`.

The handler is resolved from the DI container when an event is published, in a DI scope of its own, so it gets exactly the lifetime it was registered with:

| Lifetime | Behavior |
|---|---|
| `AddSingleton` | One instance shared by all events, created when the first event arrives. Use it for stateless handlers on hot events or for handlers that must keep state. |
| `AddTransient` | A new instance for every event. |
| `AddScoped` | A new instance for every event. Scoped dependencies such as `UserManager<ApplicationUser>` or a repository can be injected directly; they are disposed when the handler completes. |

Handlers of the same event run concurrently, each in its own scope. A handler registered as transient or scoped must not keep state in fields between events.

Registration does not construct the handler. It checks that the handler type is registered in the DI container, so a handler the module forgot to register fails at application startup with a clear message. A handler whose own dependencies cannot be resolved fails when the container is built in Development, where `ValidateOnBuild` is on, and at the first event in Production.

Earlier platform versions resolved every handler once at startup and kept that single instance for the lifetime of the application regardless of the registered lifetime. A handler written against that behavior, for example one that keeps state in fields or opens a connection in its constructor, must be registered as a singleton. That also works for a handler shipped in a module you cannot change: register the same type again as a singleton from your own module's `Initialize`; the last registration wins.

## How to raise domain events
Inject `IEventPublisher` and publish the event where the significant state change happens:

```C#
await _eventPublisher.Publish(new CustomDomainEvent { Customer = customer });
```

## How to override an existing event handler with a new derived type
```C#
//Derive a new handler from an overrided handler class
public class CustomDomainEventHandler2 : CustomDomainEventHandler
{ .... }
//Override in DI container
void Initialize(IServiceCollection serviceCollection)
{
  ...
   serviceCollection.AddTransient<CustomDomainEventHandler, CustomDomainEventHandler2>();
  ...
}
```

## How to unsubscribe an existing event handler

```C#
public void PostInitialize(IApplicationBuilder appBuilder)
{
    appBuilder.UnregisterEventHandler<CustomDomainEvent, CustomDomainEventHandler>();
}
```

The handler type is the type that was passed to `RegisterEventHandler`. A handler that was overridden in the DI container with a derived type can also be unsubscribed by that derived type, as in earlier platform versions. Unsubscribing a type that is not subscribed to the event changes nothing and logs a warning.
