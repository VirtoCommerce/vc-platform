#nullable enable
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>DI helpers for registering background-job handlers.</summary>
public static class BackgroundJobsServiceCollectionExtensions
{
    /// <summary>
    /// Registers an <see cref="IBackgroundJobHandler{TPayload}"/>. Later registrations win, so a partner module
    /// can override a handler by registering its own implementation after the base module is loaded.
    /// </summary>
    public static IServiceCollection AddBackgroundJob<TPayload, THandler>(this IServiceCollection services)
        where TPayload : class
        where THandler : class, IBackgroundJobHandler<TPayload>
    {
        services.AddTransient<IBackgroundJobHandler<TPayload>, THandler>();
        return services;
    }
}
