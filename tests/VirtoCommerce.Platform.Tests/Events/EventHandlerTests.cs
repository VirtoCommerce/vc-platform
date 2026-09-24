using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddSingleton<Handler>());
        var handler = provider.GetRequiredService<Handler>();

        // This handler should handle all event types
        applicationBuilder.RegisterEventHandler<DomainEvent, Handler>();

        // These handlers should handle only specific event types
        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();
        applicationBuilder.RegisterEventHandler<UserChangedEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);
        await publisher.Publish(new UserChangedEvent(changedEntries: null), TestContext.Current.CancellationToken);

        handler.DomainEvents.Should().BeEquivalentTo([nameof(UserLoginEvent), nameof(UserChangedEvent)]);
        handler.UserLoginEvents.Should().BeEquivalentTo([nameof(UserLoginEvent)]);
        handler.UserChangedEvents.Should().BeEquivalentTo([nameof(UserChangedEvent)]);
    }

    [Fact]
    public async Task UnregisterEventHandler()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services =>
        {
            services.AddSingleton<Handler>();
            services.AddSingleton<Handler2>();
        });

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();
        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler2>();

        applicationBuilder.UnregisterEventHandler<UserLoginEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        provider.GetRequiredService<Handler>().UserLoginEvents.Should().BeEmpty();
        provider.GetRequiredService<Handler2>().UserLoginEvents.Should().BeEquivalentTo([nameof(UserLoginEvent)]);
    }

    [Fact]
    public async Task UnregisterEventHandler_TransientOverriddenByDerivedType_ByDerivedType()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddTransient<Handler, Handler2>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();
        applicationBuilder.UnregisterEventHandler<UserLoginEvent, Handler2>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        // Only the resolution that Unregister needed to match the derived type created a handler; the event reached nobody.
        provider.GetRequiredService<Recorder>().Instances.Should().ContainSingle().Which.Should().BeOfType<Handler2>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UnregisterEventHandler_SingletonOverriddenByDerivedType_ByEitherType(bool byDeclaredType)
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddSingleton<Handler, Handler2>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        if (byDeclaredType)
        {
            applicationBuilder.UnregisterEventHandler<UserLoginEvent, Handler>();
        }
        else
        {
            applicationBuilder.UnregisterEventHandler<UserLoginEvent, Handler2>();
        }

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        provider.GetRequiredService<Handler>().UserLoginEvents.Should().BeEmpty();
    }

    [Fact]
    public async Task UnregisterEventHandler_UnknownType_KeepsRegistrations()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddTransient<Handler>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();
        applicationBuilder.UnregisterEventHandler<UserLoginEvent, Handler2>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        // One resolution while Unregister looked for the derived type, one for the event.
        var handlers = provider.GetRequiredService<Recorder>().Instances.OfType<Handler>().ToList();
        handlers.Should().HaveCount(2);
        handlers[1].UserLoginEvents.Should().BeEquivalentTo([nameof(UserLoginEvent)]);
    }

    [Fact]
    public async Task TransientHandler_IsResolvedPerEvent()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddTransient<Handler>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);
        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        var handlers = provider.GetRequiredService<Recorder>().Instances.OfType<Handler>().ToList();
        handlers.Should().HaveCount(2, "one instance per event and none at registration");
        handlers.Should().OnlyContain(x => x.UserLoginEvents.Count == 1);
    }

    [Fact]
    public async Task ScopedHandler_IsResolvedPerEvent()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddScoped<Handler>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);
        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        var handlers = provider.GetRequiredService<Recorder>().Instances.OfType<Handler>().ToList();
        handlers.Should().HaveCount(2);
        handlers.Should().OnlyContain(x => x.UserLoginEvents.Count == 1);
    }

    [Fact]
    public async Task SingletonHandler_IsResolvedOnce()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddSingleton<Handler>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);
        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        var handler = provider.GetRequiredService<Recorder>().Instances.Should().ContainSingle().Which;
        handler.Should().BeSameAs(provider.GetRequiredService<Handler>());
        ((Handler)handler).UserLoginEvents.Should().HaveCount(2);
    }

    [Fact]
    public async Task CancellableHandler_IsResolvedPerEventAndReceivesTheToken()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddTransient<CancellableHandler>());

        applicationBuilder.RegisterCancellableEventHandler<UserLoginEvent, CancellableHandler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);
        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        var handlers = provider.GetRequiredService<Recorder>().Instances.OfType<CancellableHandler>().ToList();
        handlers.Should().HaveCount(2);
        handlers.Should().OnlyContain(x => x.Token == TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task ScopedDependency_IsAliveDuringHandlingAndDisposedAfterwards()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services =>
        {
            services.AddScoped<DisposableDependency>();
            services.AddTransient<DependentHandler>();
        });
        var recorder = provider.GetRequiredService<Recorder>();

        applicationBuilder.RegisterEventHandler<UserLoginEvent, DependentHandler>();
        recorder.Disposed.Should().Be(0, "registration constructs nothing");

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        recorder.DependencyDisposedDuringHandle.Should().BeFalse();
        recorder.Disposed.Should().Be(1, "the invocation scope is disposed once the handler completes");
    }

    [Fact]
    public void MissingHandler_ThrowsAtRegistration()
    {
        var (applicationBuilder, _, _) = GetServices(_ => { });

        var act = () => applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        act.Should().Throw<InvalidOperationException>().WithMessage($"*{typeof(Handler)}*");
    }

    [Fact]
    public void RegisterEventHandler_SubscribesAStandInThatNamesTheHandlerType()
    {
        // Any registrar, custom ones included, receives an ordinary handler and can see the type it stands in for.
        var registrar = new RecordingRegistrar();
        var (applicationBuilder, _, _) = GetServices(services =>
        {
            services.AddSingleton<IEventHandlerRegistrar>(registrar);
            services.AddTransient<Handler>();
            services.AddTransient<CancellableHandler>();
        });

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();
        applicationBuilder.RegisterCancellableEventHandler<UserLoginEvent, CancellableHandler>();

        registrar.Handlers.Should().HaveCount(2);
        registrar.Handlers[0].Should().BeOfType<ScopedEventHandler<UserLoginEvent, Handler>>()
            .Which.HandlerType.Should().Be(typeof(Handler));
        registrar.Handlers[1].Should().BeOfType<ScopedCancellableEventHandler<UserLoginEvent, CancellableHandler>>()
            .Which.HandlerType.Should().Be(typeof(CancellableHandler));
    }

    [Fact]
    public async Task Publish_HandlerThrowsBeforeFirstAwait_OtherHandlersStillReceiveTheEvent()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services =>
        {
            services.AddSingleton<ThrowingHandler>();
            services.AddSingleton<Handler>();
        });

        applicationBuilder.RegisterEventHandler<UserLoginEvent, ThrowingHandler>();
        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        var act = () => publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage(ThrowingHandler.Message);
        provider.GetRequiredService<Handler>().UserLoginEvents.Should().BeEquivalentTo([nameof(UserLoginEvent)]);
    }


    private static (IApplicationBuilder, IEventPublisher, IServiceProvider) GetServices(Action<IServiceCollection> configure)
    {
        var services = new ServiceCollection();
        services.AddSingleton(new Mock<ILogger<InProcessBus>>().Object);
        services.AddSingleton<InProcessBus>();
        services.AddSingleton<IEventHandlerRegistrar>(x => x.GetRequiredService<InProcessBus>());
        services.AddSingleton<IEventPublisher>(x => x.GetRequiredService<InProcessBus>());
        services.AddSingleton<Recorder>();
        configure(services);

        var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });

        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        applicationBuilderMock.Setup(x => x.ApplicationServices).Returns(provider);

        return (applicationBuilderMock.Object, provider.GetRequiredService<IEventPublisher>(), provider);
    }

    public class Recorder
    {
        public List<object> Instances { get; } = [];
        public int Disposed { get; set; }
        public bool DependencyDisposedDuringHandle { get; set; }
    }

    public class Handler : IEventHandler<DomainEvent>, IEventHandler<UserLoginEvent>, IEventHandler<UserChangedEvent>
    {
        public readonly List<string> DomainEvents = [];
        public readonly List<string> UserLoginEvents = [];
        public readonly List<string> UserChangedEvents = [];

        public Handler(Recorder recorder)
        {
            recorder.Instances.Add(this);
        }

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

    public class Handler2(Recorder recorder) : Handler(recorder);

    public class CancellableHandler : ICancellableEventHandler<UserLoginEvent>
    {
        public CancellationToken Token { get; private set; }

        public CancellableHandler(Recorder recorder)
        {
            recorder.Instances.Add(this);
        }

        public Task Handle(UserLoginEvent message, CancellationToken cancellationToken)
        {
            Token = cancellationToken;
            return Task.CompletedTask;
        }
    }

    // Throws synchronously, before returning a task.
    public class ThrowingHandler : IEventHandler<UserLoginEvent>
    {
        public const string Message = "Handler failed";

        public Task Handle(UserLoginEvent message)
        {
            throw new InvalidOperationException(Message);
        }
    }

    public sealed class DisposableDependency(Recorder recorder) : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
            recorder.Disposed++;
        }
    }

    public class DependentHandler(DisposableDependency dependency, Recorder recorder) : IEventHandler<UserLoginEvent>
    {
        public Task Handle(UserLoginEvent message)
        {
            recorder.DependencyDisposedDuringHandle = dependency.IsDisposed;
            return Task.CompletedTask;
        }
    }

    public sealed class RecordingRegistrar : IEventHandlerRegistrar
    {
        public List<object> Handlers { get; } = [];

        public void RegisterEventHandler<T>(IEventHandler<T> handler)
            where T : IEvent
        {
            Handlers.Add(handler);
        }

        public void RegisterEventHandler<T>(ICancellableEventHandler<T> handler)
            where T : IEvent
        {
            Handlers.Add(handler);
        }

        public void UnregisterEventHandler<T>(Type handlerType = null)
            where T : IEvent
        {
            // Not exercised by these tests.
        }

        public void UnregisterAllEventHandlers()
        {
            // Not exercised by these tests.
        }
    }
}
