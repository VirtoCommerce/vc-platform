#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Static, Hangfire-style convenience for enqueuing background jobs <b>without injecting <see cref="IBackgroundJob"/></b>.
/// <para>
/// This is a <b>migration aid</b> for code moving off the static <c>Hangfire.BackgroundJob.Enqueue</c> API: it lets a
/// client module enqueue work without threading <see cref="IBackgroundJob"/> through its constructors. New code should
/// prefer injecting <see cref="IBackgroundJob"/> (explicit dependency, trivially testable, no ambient state).
/// </para>
/// <para>
/// It is backed by the application's root <see cref="IServiceProvider"/>, captured once at startup via
/// <see cref="Initialize"/> (the <c>VirtoCommerce.BackgroundJobs</c> module does this). Each call opens a short-lived
/// DI scope to resolve the scoped <see cref="IBackgroundJob"/> facade — mirroring how the recurring scheduler bridges
/// to the scoped facade. The current user still flows into that scope via <c>IHttpContextAccessor</c> (AsyncLocal), so
/// the enqueuing user is recorded exactly as with a directly-injected <see cref="IBackgroundJob"/>.
/// </para>
/// </summary>
public static class BackgroundJob
{
    private static volatile IServiceProvider? _rootServiceProvider;

    /// <summary>
    /// Wires the static facade to the application's root service provider. Called once by the platform host during
    /// startup (see the BackgroundJobs module's <c>PlatformStartup.Configure</c>); not intended for application code.
    /// </summary>
    /// <param name="rootServiceProvider">The application's root service provider.</param>
    public static void Initialize(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider ?? throw new ArgumentNullException(nameof(rootServiceProvider));
    }

    public static async Task<string> Enqueue<THandler>(object payload, EnqueueOptions? options = null,
        CancellationToken cancellationToken = default)
        where THandler : class
    {
        var provider = _rootServiceProvider
            ?? throw new InvalidOperationException(
                "BackgroundJob static facade is not initialized. Install the VirtoCommerce.BackgroundJobs module " +
                "(it calls BackgroundJob.Initialize during startup), or inject IBackgroundJob instead.");

        using var scope = provider.CreateScope();
        var backgroundJob = scope.ServiceProvider.GetRequiredService<IBackgroundJob>();
        return await backgroundJob.Enqueue<THandler>(payload, options, cancellationToken);
    }
}
