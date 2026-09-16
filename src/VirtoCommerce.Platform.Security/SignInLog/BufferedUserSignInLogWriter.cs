using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
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
/// </summary>
public class BufferedUserSignInLogWriter : BackgroundService, IUserSignInLogWriter
{
    private readonly IUserSignInLogService _service;
    private readonly ILogger<BufferedUserSignInLogWriter> _logger;
    private readonly Channel<UserSignInLog> _channel;
    private readonly TimeSpan _flushInterval;
    private readonly int _batchSize;

    private int _droppedCount;

    public BufferedUserSignInLogWriter(
        IUserSignInLogService service,
        ILogger<BufferedUserSignInLogWriter> logger,
        IOptions<SignInLogOptions> options)
    {
        var settings = options?.Value ?? new SignInLogOptions();

        _service = service;
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

    public virtual async Task Flush(CancellationToken cancellationToken)
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

    private async Task PersistAsync(List<UserSignInLog> batch, CancellationToken cancellationToken)
    {
        try
        {
            await _service.SaveChanges(batch, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist {Count} sign-in audit record(s).", batch.Count);
        }
    }
}
