using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Bus;

namespace VirtoCommerce.Platform.Core.Events;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder RegisterHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : IEventHandler<TEvent>
    {
        var handlerRegistrar = applicationBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
        var handler = applicationBuilder.ApplicationServices.GetService<THandler>();

        handlerRegistrar.RegisterHandler<TEvent>(handler.Handle);

        return applicationBuilder;
    }

    public static IApplicationBuilder RegisterCancellableHandler<TEvent, THandler>(this IApplicationBuilder applicationBuilder)
        where TEvent : IEvent
        where THandler : ICancellableEventHandler<TEvent>
    {
        var handlerRegistrar = applicationBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
        var handler = applicationBuilder.ApplicationServices.GetService<THandler>();

        handlerRegistrar.RegisterHandler<TEvent>(handler.Handle);

        return applicationBuilder;
    }
}
