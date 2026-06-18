#nullable enable
using System;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;

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

    /// <summary>
    /// Registers an <see cref="IBackgroundJobHandler{TPayload}"/> AND a recurring (cron) schedule for it. On each
    /// occurrence the background-job engine's scheduler enqueues a fresh <typeparamref name="TPayload"/> (created via
    /// <see cref="AbstractTypeFactory{TPayload}"/>) through <see cref="IBackgroundJob"/>, so the same handler runs on
    /// the active engine. Configure a fixed cron or a setting-driven schedule via <paramref name="configure"/>.
    /// </summary>
    public static IServiceCollection AddRecurringJob<TPayload, THandler>(
        this IServiceCollection services, Action<IRecurringJobScheduleBuilder> configure)
        where TPayload : class
        where THandler : class, IBackgroundJobHandler<TPayload>
    {
        ArgumentNullException.ThrowIfNull(configure);

        services.AddBackgroundJob<TPayload, THandler>();

        var builder = new RecurringJobScheduleBuilder();
        configure(builder);

        var registration = builder.Build((jobs, options, cancellationToken) =>
            jobs.Enqueue(AbstractTypeFactory<TPayload>.TryCreateInstance(), options, cancellationToken));

        services.AddSingleton(registration);

        return services;
    }
}
