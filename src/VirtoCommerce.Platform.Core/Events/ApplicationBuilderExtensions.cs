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
        var handler = applicationBuilder.ApplicationServices.GetRequiredService<THandler>();
        return applicationBuilder.RegisterEventHandler<TEvent>(handler.Handle);
    }

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
        var handler = applicationBuilder.ApplicationServices.GetRequiredService<THandler>();
        return applicationBuilder.RegisterCancellableEventHandler<TEvent>(handler.Handle);
    }

    public static IApplicationBuilder RegisterCancellableEventHandler<TEvent>(this IApplicationBuilder applicationBuilder, Func<TEvent, CancellationToken, Task> handler)
        where TEvent : IEvent
    {
        var registrar = applicationBuilder.ApplicationServices.GetRequiredService<IEventHandlerRegistrar>();
        registrar.RegisterEventHandler(handler);
        return applicationBuilder;
    }
}
