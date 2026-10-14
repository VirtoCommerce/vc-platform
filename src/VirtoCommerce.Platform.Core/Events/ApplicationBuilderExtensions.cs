using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Events;

public static class ApplicationBuilderExtensions
{
    // The handler is not constructed here. A ScopedEventHandler stands in for it and resolves it from a DI scope of its own
    // for every event, so the handler gets exactly the lifetime it was registered with.
    public static IApplicationBuilder RegisterEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>
    {
        var services = applicationBuilder.ApplicationServices;
        CheckRegistered<THandler>(services);

        var registrar = services.GetRequiredService<IEventHandlerRegistrar>();
        registrar.RegisterEventHandler<TEvent>(new ScopedEventHandler<TEvent, THandler>(services.GetRequiredService<IServiceScopeFactory>()));
        return applicationBuilder;
    }

    public static IApplicationBuilder RegisterCancellableEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : ICancellableEventHandler<TEvent>
    {
        var services = applicationBuilder.ApplicationServices;
        CheckRegistered<THandler>(services);

        var registrar = services.GetRequiredService<IEventHandlerRegistrar>();
        registrar.RegisterEventHandler<TEvent>(new ScopedCancellableEventHandler<TEvent, THandler>(services.GetRequiredService<IServiceScopeFactory>()));
        return applicationBuilder;
    }

    public static IApplicationBuilder UnregisterEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        registrar.UnregisterEventHandler<TEvent>(typeof(THandler));
        return applicationBuilder;
    }

    public static IApplicationBuilder UnregisterCancellableEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : ICancellableEventHandler<TEvent>
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        registrar.UnregisterEventHandler<TEvent>(typeof(THandler));
        return applicationBuilder;
    }

    public static IApplicationBuilder UnregisterEventHandlers<TEvent>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        registrar.UnregisterEventHandler<TEvent>();
        return applicationBuilder;
    }

    public static IApplicationBuilder UnregisterAllEventHandlers(this IApplicationBuilder applicationBuilder)
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        registrar.UnregisterAllEventHandlers();
        return applicationBuilder;
    }

    // A handler the module forgot to add to the service collection fails here, at startup, without constructing anything.
    private static void CheckRegistered<THandler>(IServiceProvider services)
    {
        var isService = services.GetService<IServiceProviderIsService>();

        if (isService != null && !isService.IsService(typeof(THandler)))
        {
            throw new InvalidOperationException($"Event handler '{typeof(THandler)}' is not registered in the service collection.");
        }
    }
}
