#nullable enable
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>DI helpers for registering background-job handlers.</summary>
public static class BackgroundJobsServiceCollectionExtensions
{
    // The two-generic-parameter AddRecurringJob(services, configure) overload, used by AddRecurringJob<THandler> to
    // delegate once the payload type is inferred (reusing AbstractTypeFactory creation + schedule building).
    private static readonly MethodInfo _addRecurringJobConfigureMethod = typeof(BackgroundJobsServiceCollectionExtensions)
        .GetMethods()
        .Single(m => m.Name == nameof(AddRecurringJob)
            && m.IsGenericMethodDefinition
            && m.GetGenericArguments().Length == 2
            && m.GetParameters().Length == 2);

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
    /// Registers a handler, inferring the payload type(s) from the <see cref="IBackgroundJobHandler{TPayload}"/>
    /// interface(s) it implements — a convenience over the two-type-parameter overload. If the handler implements the
    /// interface for several payloads, all are registered. Later registrations win (partner override).
    /// <para>Prefer <see cref="AddBackgroundJob{TPayload, THandler}"/> when you already know the payload — it is
    /// compile-time checked and trim/AOT-friendly; this overload resolves the payload via reflection.</para>
    /// </summary>
    public static IServiceCollection AddBackgroundJob<THandler>(this IServiceCollection services)
        where THandler : class
    {
        var handlerType = typeof(THandler);

        // GetInterfaces returns the CLOSED generics (IBackgroundJobHandler<SomePayload>) — register them directly.
        var handlerInterfaces = handlerType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBackgroundJobHandler<>))
            .ToArray();

        if (handlerInterfaces.Length == 0)
        {
            throw new ArgumentException($"{handlerType.Name} must implement IBackgroundJobHandler<TPayload>.", nameof(THandler));
        }

        foreach (var serviceType in handlerInterfaces)
        {
            services.AddTransient(serviceType, handlerType);
        }

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
        => services.AddRecurringJob<TPayload, THandler>(
            () => AbstractTypeFactory<TPayload>.TryCreateInstance(), configure);

    /// <summary>
    /// Registers a recurring (cron) job, inferring the payload type from the handler's
    /// <see cref="IBackgroundJobHandler{TPayload}"/> interface — a convenience over the two-type-parameter overload.
    /// Each occurrence enqueues a payload created via <see cref="AbstractTypeFactory{TPayload}"/>. The handler must
    /// implement the interface for exactly one payload; to pass parameters or pick among several payloads, use
    /// <see cref="AddRecurringJob{TPayload, THandler}(IServiceCollection, Func{TPayload}, Action{IRecurringJobScheduleBuilder})"/>.
    /// </summary>
    public static IServiceCollection AddRecurringJob<THandler>(
        this IServiceCollection services, Action<IRecurringJobScheduleBuilder> configure)
        where THandler : class
    {
        ArgumentNullException.ThrowIfNull(configure);

        var payloadType = GetSingleHandlerPayloadType(typeof(THandler));
        var register = _addRecurringJobConfigureMethod.MakeGenericMethod(payloadType, typeof(THandler));

        try
        {
            return (IServiceCollection)register.Invoke(null, [services, configure])!;
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            // Surface the real error (e.g. invalid schedule) instead of the reflection wrapper, preserving its stack.
            ExceptionDispatchInfo.Throw(ex.InnerException);
            throw; // unreachable
        }
    }

    /// <summary>
    /// Registers an <see cref="IBackgroundJobHandler{TPayload}"/> AND a recurring (cron) schedule that enqueues a
    /// payload built by <paramref name="payloadFactory"/>. Use this overload to pass parameters to the recurring job
    /// (e.g. <c>() =&gt; new SendDigestPayload { Top = 10 }</c>). The factory is invoked <b>once per occurrence</b>, so
    /// it may also produce per-run values. To keep the payload partner-overridable, build it via
    /// <see cref="AbstractTypeFactory{TPayload}"/> inside the factory. Configure a fixed cron or a setting-driven
    /// schedule via <paramref name="configure"/>.
    /// </summary>
    public static IServiceCollection AddRecurringJob<TPayload, THandler>(
        this IServiceCollection services, Func<TPayload> payloadFactory, Action<IRecurringJobScheduleBuilder> configure)
        where TPayload : class
        where THandler : class, IBackgroundJobHandler<TPayload>
    {
        ArgumentNullException.ThrowIfNull(payloadFactory);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddBackgroundJob<TPayload, THandler>();

        var builder = new RecurringJobScheduleBuilder();
        configure(builder);

        var registration = builder.Build((jobs, options, cancellationToken) =>
            jobs.Enqueue(payloadFactory(), options, cancellationToken));

        services.AddSingleton(registration);

        return services;
    }

    // Resolves the single payload type a handler is registered for. A recurring schedule targets exactly one payload,
    // so a handler implementing IBackgroundJobHandler<> for none or several is rejected.
    private static Type GetSingleHandlerPayloadType(Type handlerType)
    {
        var payloadTypes = handlerType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBackgroundJobHandler<>))
            .Select(i => i.GetGenericArguments()[0])
            .ToArray();

        return payloadTypes.Length switch
        {
            1 => payloadTypes[0],
            0 => throw new ArgumentException($"{handlerType.Name} must implement IBackgroundJobHandler<TPayload>."),
            _ => throw new ArgumentException(
                $"{handlerType.Name} implements IBackgroundJobHandler<> for multiple payloads; use AddRecurringJob<TPayload, THandler>(...) to choose one."),
        };
    }
}
