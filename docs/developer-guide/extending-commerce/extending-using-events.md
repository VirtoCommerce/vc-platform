---
title: Extending using events
description: The article describes a process of extending Virto Commerce using events
layout: docs
date: 2018-04-18
priority: 7
---
## Events overview

An event is something that has happened in the past. A domain event is, logically, something that happened in a particular domain, and something you want other parts of the same domain (in-process) to be aware of and potentially react to.

An important benefit of domain events is that side effects after something happened in a domain can be expressed explicitly instead of implicitly. Those side effects must be consistent so either all the operations related to the business task happen, or none of them. In addition, domain events enable a better separation of concerns among classes within the same domain.

## How to define domain events
A domain event is just a simple POCO that represents an interesting occurence in the domain.
```
public class CustomDomainEvent : DomainEvent
{
 public Customer Customer { get; set; }
}
```

## How to define a new event handler 

```
public class CutomDomainEventHandler : IEventHandler<CustomDomainEvent>
{
  public async Task Handle(CustomDomainEventmessage)
  {
    //Some logic here
  }
}
```

## How to register an event handler, subscribe to a domain event

```
var eventHandlerRegistrar = _container.Resolve<IHandlerRegistrar>();
eventHandlerRegistrar.RegisterHandler<CustomDomainEvent>(async (message, token) => await _container.Resolve<CustomDomainEventHandler>().Handle(message));
```

## How to raise domain events
In your domain entities, when a significant state change happens youвЂ™ll want to raise your domain events like this
```
var eventPublisher = _container.Resolve<IEventPublisher>();
eventPublisher .Publish(new CustomDomainEvent()));
```

## How to override an existing event handler with a new derived type
```
public class CustomDomainEventHandler2 : CustomDomainEventHandler
{ .... }
//Override in IoC container
_container.RegisterType<CustomDomainEventHandler, CustomDomainEventHandler2>("CustomDomainEventHandler")
```
