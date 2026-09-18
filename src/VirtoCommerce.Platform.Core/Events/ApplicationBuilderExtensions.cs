using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace VirtoCommerce.Platform.Core.Events;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder RegisterEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>
    {
        var services = applicationBuilder.ApplicationServices;
        var registrar = services.GetRequiredService<IEventHandlerRegistrar>();

        if (ResolveHandlersPerInvocation(services))
        {
            registrar.RegisterEventHandler<TEvent, THandler>(services);
        }
        else
        {
            registrar.RegisterEventHandler<TEvent>(services.GetRequiredService<THandler>());
        }

        return applicationBuilder;
    }

    public static IApplicationBuilder RegisterCancellableEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : ICancellableEventHandler<TEvent>
    {
        var services = applicationBuilder.ApplicationServices;
        var registrar = services.GetRequiredService<IEventHandlerRegistrar>();

        if (ResolveHandlersPerInvocation(services))
        {
            registrar.RegisterCancellableEventHandler<TEvent, THandler>(services);
        }
        else
        {
            registrar.RegisterEventHandler<TEvent>(services.GetRequiredService<THandler>());
        }

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

    private static bool ResolveHandlersPerInvocation(IServiceProvider services)
    {
        return services.GetService<IOptions<EventHandlerOptions>>()?.Value.ResolveHandlersPerInvocation ?? true;
    }
}
