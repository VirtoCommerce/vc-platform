
An event is something that has happened in the past. A domain event is, logically, something that happened in a particular domain, and something you want other parts of the same domain (in-process) to be aware of and potentially react to.

An important benefit of domain events is that side effects after something happened in a domain can be expressed explicitly instead of implicitly. Those side effects must be consistent so either all the operations related to the business task happen, or none of them. In addition, domain events enable a better separation of concerns among classes within the same domain.

## How to define domain events
A domain event is just a simple POCO type that represents an interesting occurence in the domain.

```C#
public class CustomDomainEvent : DomainEvent
{
 public Customer Customer { get; set; }
}
```

## How to define a new event handler 

```C#
public class CutomDomainEventHandler : IEventHandler<CustomDomainEvent>
{
  public async Task Handle(CustomDomainEventmessage)
  {
    //Some logic here
  }
}
```

## How to register an event handler, subscribe to a domain event

```C#
void  Initialize(IServiceCollection serviceCollection)
{
  ...
   serviceCollection.AddTransient<CustomDomainEventHandler>();
  ...
}

void PostInitialize(IApplicationBuilder appBuilder)
{
  ...
var eventHandlerRegistrar = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
eventHandlerRegistrar.RegisterHandler<CustomDomainEvent>((message, token) => appBuilder.ApplicationServices.GetService<CustomDomainEventHandler>().Handle(message));
  ...
}
```

## How to raise domain events
In your domain entities, when a significant state change happens you’ll want to raise your domain events like this
```
var eventPublisher = _container.Resolve<IEventPublisher>();
eventPublisher.Publish(new CustomDomainEvent()));
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
