using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Moq;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security.Events;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Events;

[Trait("Category", "Unit")]
public class EventHandlerTests
{
    [Fact]
    public async Task HandleMultipleEventTypesWithSingleRegistration()
    {
        var handler = new Handler();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(x => x.GetService(typeof(Handler))).Returns(handler);

        var (applicationBuilder, publisher) = GetServices(serviceProviderMock);

        // This handler should handle all event types
        applicationBuilder.RegisterEventHandler<DomainEvent, Handler>();

        // These handlers should handle only specific event types
        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();
        applicationBuilder.RegisterEventHandler<UserChangedEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null));
        await publisher.Publish(new UserChangedEvent(changedEntries: null));

        handler.DomainEvents.Should().BeEquivalentTo([nameof(UserLoginEvent), nameof(UserChangedEvent)]);
        handler.UserLoginEvents.Should().BeEquivalentTo([nameof(UserLoginEvent)]);
        handler.UserChangedEvents.Should().BeEquivalentTo([nameof(UserChangedEvent)]);
    }

    [Fact]
    public async Task UnregisterEventHandler()
    {
        var handler1 = new Handler();
        var handler2 = new Handler2();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(x => x.GetService(typeof(Handler))).Returns(handler1);
        serviceProviderMock.Setup(x => x.GetService(typeof(Handler2))).Returns(handler2);

        var (applicationBuilder, publisher) = GetServices(serviceProviderMock);

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();
        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler2>();

        applicationBuilder.UnregisterEventHandler<UserLoginEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null));

        handler1.UserLoginEvents.Should().BeEmpty();
        handler2.UserLoginEvents.Should().BeEquivalentTo([nameof(UserLoginEvent)]);
    }


    private static (IApplicationBuilder, IEventPublisher) GetServices(Mock<IServiceProvider> serviceProviderMock = null)
    {
        serviceProviderMock ??= new Mock<IServiceProvider>();

        var bus = new InProcessBus(new Mock<ILogger<InProcessBus>>().Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(IEventHandlerRegistrar))).Returns(bus);
        serviceProviderMock.Setup(x => x.GetService(typeof(IEventPublisher))).Returns(bus);

        var applicationBuilderMock = new Mock<IApplicationBuilder>();

        applicationBuilderMock.Setup(x => x.ApplicationServices).Returns(serviceProviderMock.Object);

        return (applicationBuilderMock.Object, bus);
    }

    private class Handler2 : Handler;

    private class Handler : IEventHandler<DomainEvent>, IEventHandler<UserLoginEvent>, IEventHandler<UserChangedEvent>
    {
        public readonly List<string> DomainEvents = [];
        public readonly List<string> UserLoginEvents = [];
        public readonly List<string> UserChangedEvents = [];

        public Task Handle(DomainEvent message)
        {
            DomainEvents.Add(message.GetType().Name);
            return Task.CompletedTask;
        }

        public Task Handle(UserLoginEvent message)
        {
            UserLoginEvents.Add(message.GetType().Name);
            return Task.CompletedTask;
        }

        public Task Handle(UserChangedEvent message)
        {
            UserChangedEvents.Add(message.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
