using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Events;

public static class ApplicationBuilderExtensions
{
    // The handler is resolved per event with the lifetime it was registered with; register it as a singleton when one shared instance is intended.
    public static IApplicationBuilder RegisterEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>
    {
        var services = applicationBuilder.ApplicationServices;
        services.GetRequiredService<IEventHandlerRegistrar>().RegisterEventHandler<TEvent, THandler>(services);
        return applicationBuilder;
    }

    public static IApplicationBuilder RegisterCancellableEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : ICancellableEventHandler<TEvent>
    {
        var services = applicationBuilder.ApplicationServices;
        services.GetRequiredService<IEventHandlerRegistrar>().RegisterCancellableEventHandler<TEvent, THandler>(services);
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
}
