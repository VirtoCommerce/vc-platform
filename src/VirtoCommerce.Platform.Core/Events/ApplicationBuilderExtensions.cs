using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Events;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder RegisterEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        var handler = applicationBuilder.ApplicationServices.GetRequiredService<THandler>();
        registrar.RegisterEventHandler(handler);
        return applicationBuilder;
    }

    [Obsolete("Use IApplicationBuilder.RegisterEventHandler<TEvent, THandler>()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public static IApplicationBuilder RegisterEventHandler<TEvent>(this IApplicationBuilder applicationBuilder, Func<TEvent, Task> handler)
        where TEvent : IEvent
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        registrar.RegisterEventHandler(handler);
        return applicationBuilder;
    }

    public static IApplicationBuilder RegisterCancellableEventHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : ICancellableEventHandler<TEvent>
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        var handler = applicationBuilder.ApplicationServices.GetRequiredService<THandler>();
        registrar.RegisterEventHandler(handler);
        return applicationBuilder;
    }

    [Obsolete("Use IApplicationBuilder.RegisterEventHandler<TEvent, THandler>()", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public static IApplicationBuilder RegisterCancellableEventHandler<TEvent>(this IApplicationBuilder applicationBuilder, Func<TEvent, CancellationToken, Task> handler)
        where TEvent : IEvent
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        registrar.RegisterEventHandler(handler);
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
