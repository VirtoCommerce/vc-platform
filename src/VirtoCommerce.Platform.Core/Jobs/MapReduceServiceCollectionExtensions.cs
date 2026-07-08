using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// DI helpers a consumer module calls to register its map and reduce handlers. Shipped in Platform.Core so a module
/// needs no compile-time dependency on the Background Jobs module. The engine-agnostic coordinators and the
/// <see cref="IMapReduceJob"/> facade are registered once by the Background Jobs module (its <c>AddMapReduceCore</c>).
/// </summary>
public static class MapReduceServiceCollectionExtensions
{
    /// <summary>
    /// Registers a module's map and reduce handlers for one batch type. Call from a module's
    /// <c>Initialize(IServiceCollection)</c>.
    /// </summary>
    public static IServiceCollection AddMapReduceJob<TItem, TResult, TState, TMap, TReduce>(this IServiceCollection services)
        where TItem : class
        where TResult : class
        where TState : class
        where TMap : class, IMapJobHandler<TItem, TResult>
        where TReduce : class, IReduceJobHandler<TState, TResult>
    {
        // Register the concrete handlers so the coordinators can resolve them by type (handler-explicit enqueue), and
        // bind the closed interfaces to the same instances for the interface-based fallback path.
        services.AddTransient<TMap>();
        services.AddTransient<TReduce>();
        services.AddTransient<IMapJobHandler<TItem, TResult>>(sp => sp.GetRequiredService<TMap>());
        services.AddTransient<IReduceJobHandler<TState, TResult>>(sp => sp.GetRequiredService<TReduce>());
        return services;
    }

    /// <summary>
    /// Registers a module's map and reduce handlers, inferring the item/result/state types from the handler
    /// interfaces — a convenience over the five-type-parameter overload. Validates that the map and reduce handlers
    /// agree on the result type. Use <see cref="AddMapReduceJob{TItem, TResult, TState, TMap, TReduce}"/> when you
    /// prefer to state the types explicitly (compile-time checked, trim/AOT-friendly).
    /// </summary>
    public static IServiceCollection AddMapReduceJob<TMap, TReduce>(this IServiceCollection services)
        where TMap : class
        where TReduce : class
    {
        // Resolve the CLOSED handler interfaces (and validate the pair agrees on the result type) via the shared helper.
        var (mapInterface, reduceInterface) = MapReduceHandlerTypes.ResolveInterfaces(typeof(TMap), typeof(TReduce));

        // Register the concrete handlers (so the coordinators resolve them by type) plus the interface fallbacks.
        services.AddTransient<TMap>();
        services.AddTransient<TReduce>();
        services.AddTransient(mapInterface, sp => sp.GetRequiredService<TMap>());
        services.AddTransient(reduceInterface, sp => sp.GetRequiredService<TReduce>());
        return services;
    }
}
