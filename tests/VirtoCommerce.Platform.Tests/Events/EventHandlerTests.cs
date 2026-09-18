using System;
using System.Collections.Generic;
using System.Linq;
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
    public async Task UnregisterEventHandler_ByImplementationType()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddTransient<Handler, Handler2>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();
        applicationBuilder.UnregisterEventHandler<UserLoginEvent, Handler2>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        // Only the probe at registration created a handler; the event reached nobody.
        provider.GetRequiredService<Recorder>().Instances.Should().ContainSingle();
    }

    [Fact]
    public async Task TransientHandler_IsResolvedPerEvent()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddTransient<Handler>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);
        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        var handlers = provider.GetRequiredService<Recorder>().Instances.OfType<Handler>().ToList();
        handlers.Should().HaveCount(3, "one probe at registration plus one instance per event");
        handlers.Skip(1).Should().OnlyContain(x => x.UserLoginEvents.Count == 1);
    }

    [Fact]
    public async Task ScopedHandler_IsResolvedPerEvent()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services => services.AddScoped<Handler>());

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);
        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        var handlers = provider.GetRequiredService<Recorder>().Instances.OfType<Handler>().ToList();
        handlers.Should().HaveCount(3);
        handlers.Skip(1).Should().OnlyContain(x => x.UserLoginEvents.Count == 1);
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
    public async Task ScopedDependency_IsAliveDuringHandlingAndDisposedAfterwards()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services =>
        {
            services.AddScoped<DisposableDependency>();
            services.AddTransient<DependentHandler>();
        });
        var recorder = provider.GetRequiredService<Recorder>();

        applicationBuilder.RegisterEventHandler<UserLoginEvent, DependentHandler>();
        recorder.Disposed.Should().Be(1, "the probe scope used at registration is disposed immediately");

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        recorder.DependencyDisposedDuringHandle.Should().BeFalse();
        recorder.Disposed.Should().Be(2, "the invocation scope is disposed once the handler completes");
    }

    [Fact]
    public async Task LegacyMode_TransientHandlerIsResolvedOnce()
    {
        var (applicationBuilder, publisher, provider) = GetServices(services =>
        {
            services.AddTransient<Handler>();
            services.Configure<EventHandlerOptions>(options => options.ResolveHandlersPerInvocation = false);
        });

        applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);
        await publisher.Publish(new UserLoginEvent(user: null), TestContext.Current.CancellationToken);

        var handler = provider.GetRequiredService<Recorder>().Instances.Should().ContainSingle().Which;
        ((Handler)handler).UserLoginEvents.Should().HaveCount(2);
    }

    [Fact]
    public void MissingHandler_ThrowsAtRegistration()
    {
        var (applicationBuilder, _, _) = GetServices(_ => { });

        var act = () => applicationBuilder.RegisterEventHandler<UserLoginEvent, Handler>();

        act.Should().Throw<InvalidOperationException>();
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

        // Startup registers the collection itself, which is how the bus learns the declared lifetime of a handler.
        services.AddSingleton<IServiceCollection>(services);

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

    public class DisposableDependency(Recorder recorder) : IDisposable
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
}
