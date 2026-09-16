using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Security.SignInLog;

/// <summary>
/// Buffers audit rows in a bounded channel and flushes them in batches on a background loop.
/// Keeps the database off the sign-in latency path and stops an unauthenticated endpoint from
/// becoming a write amplifier during a credential-stuffing run.
/// Enrichment also runs here rather than on the request thread, so a module's enricher cannot add
/// latency to a sign-in - see <see cref="Enrich"/>.
/// </summary>
public class BufferedUserSignInLogWriter : BackgroundService, IUserSignInLogWriter
{
    private readonly IUserSignInLogService _service;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BufferedUserSignInLogWriter> _logger;
    private readonly Channel<UserSignInLog> _channel;
    private readonly SemaphoreSlim _flushLock = new(1, 1);
    private readonly TimeSpan _flushInterval;
    private readonly int _batchSize;

    private int _droppedCount;

    public BufferedUserSignInLogWriter(
        IUserSignInLogService service,
        IServiceScopeFactory scopeFactory,
        ILogger<BufferedUserSignInLogWriter> logger,
        IOptions<SignInLogOptions> options)
    {
        var settings = options?.Value ?? new SignInLogOptions();

        _service = service;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _batchSize = Math.Max(1, settings.BatchSize);
        _flushInterval = TimeSpan.FromSeconds(Math.Max(1, settings.FlushIntervalSeconds));
        _channel = Channel.CreateBounded<UserSignInLog>(new BoundedChannelOptions(Math.Max(1, settings.BufferCapacity))
        {
            // Wait is the only mode where TryWrite reports a rejection: every Drop* mode returns
            // true and discards silently. Since the caller uses TryWrite and never WriteAsync, this
            // never blocks a sign-in - it just makes the loss counter trustworthy, which an audit
            // buffer needs more than it needs to keep the newest rows of an extreme burst.
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
        });
    }

    /// <summary>
    /// Rows discarded because the buffer was full. Approximate under concurrent draining — the
    /// channel gives no eviction callback — but never silently zero when drops happened.
    /// </summary>
    public int DroppedCount => _droppedCount;

    public void Write(UserSignInLog record)
    {
        if (record is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(record.Id))
        {
            record.Id = Guid.NewGuid().ToString("N");
        }

        if (record.CreatedDate == default)
        {
            record.CreatedDate = DateTime.UtcNow;
        }

        // Exact: with FullMode.Wait, TryWrite returns false only when the buffer is genuinely full.
        // Inferring drops from queue depth was wrong - the background reader drains concurrently,
        // so a falling count is not evidence of an eviction.
        if (!_channel.Writer.TryWrite(record))
        {
            var dropped = Interlocked.Increment(ref _droppedCount);
            _logger.LogWarning("Sign-in audit buffer full; dropped {DroppedCount} record(s) so far.", dropped);
        }
    }

    /// <summary>
    /// Drains the buffer. Public so a host can flush on demand, which is exactly why it has to take a
    /// lock: the channel is configured <c>SingleReader</c>, and an on-demand call racing the timer loop
    /// would put two readers on it. The wait itself is deliberately not cancellable - a shutdown drain
    /// must not abandon the queue to a flush that is already running - while the token still bounds
    /// the work inside.
    /// </summary>
    public virtual async Task Flush(CancellationToken cancellationToken)
    {
        await _flushLock.WaitAsync(CancellationToken.None);

        try
        {
            await FlushCore(cancellationToken);
        }
        finally
        {
            _flushLock.Release();
        }
    }

    private async Task FlushCore(CancellationToken cancellationToken)
    {
        var batch = new List<UserSignInLog>(_batchSize);

        while (!cancellationToken.IsCancellationRequested && _channel.Reader.TryRead(out var record))
        {
            batch.Add(record);

            if (batch.Count >= _batchSize)
            {
                await PersistAsync(batch, cancellationToken);
                batch = new List<UserSignInLog>(_batchSize);
            }
        }

        if (batch.Count > 0)
        {
            await PersistAsync(batch, cancellationToken);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_flushInterval);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            await Flush(stoppingToken);
        }

        // Drain what is left so a graceful shutdown does not lose the tail of the trail. The host
        // bounds this with its own stop timeout, so it cannot hang shutdown indefinitely.
        await Flush(CancellationToken.None);
    }

    /// <summary>
    /// Enrichers are the extension point for modules - a customer module resolves the store and
    /// organization names the platform cannot see. They run here, on the flush loop, and never on the
    /// request thread: awaiting a module's database call during a sign-in would put it back on the
    /// latency path and reopen the user-enumeration timing gap that <c>DelayedResponse</c> closes,
    /// because an enricher gated on a resolved user id does work only when the account exists.
    /// One scope per batch, not per row. A broken enricher must never lose a row, so failures are
    /// logged and the batch is persisted with whatever enrichment succeeded.
    /// </summary>
    protected virtual async Task Enrich(IList<UserSignInLog> batch)
    {
        using var scope = _scopeFactory.CreateScope();

        var enrichers = scope.ServiceProvider
            .GetServices<IUserSignInLogEnricher>()
            .OrderBy(x => x.Priority)
            .ToList();

        if (enrichers.Count == 0)
        {
            return;
        }

        foreach (var record in batch)
        {
            foreach (var enricher in enrichers)
            {
                try
                {
                    await enricher.Enrich(record);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Sign-in log enricher {Enricher} failed.", enricher.GetType().Name);
                }
            }
        }
    }

    private async Task PersistAsync(List<UserSignInLog> batch, CancellationToken cancellationToken)
    {
        try
        {
            await Enrich(batch);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enrich {Count} sign-in audit record(s).", batch.Count);
        }

        try
        {
            await _service.SaveChanges(batch, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist {Count} sign-in audit record(s).", batch.Count);
        }
    }

    public override void Dispose()
    {
        _flushLock.Dispose();
        base.Dispose();
    }
}
