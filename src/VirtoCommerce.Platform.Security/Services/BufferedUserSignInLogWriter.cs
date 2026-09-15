using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Services;

/// <summary>
/// Buffers audit rows in a bounded channel and flushes them in batches on a background loop.
/// Keeps the database off the sign-in latency path and stops an unauthenticated endpoint from
/// becoming a write amplifier during a credential-stuffing run.
/// </summary>
public class BufferedUserSignInLogWriter : BackgroundService, IUserSignInLogWriter
{
    private static readonly TimeSpan _flushInterval = TimeSpan.FromSeconds(5);

    private readonly IUserSignInLogService _service;
    private readonly ILogger<BufferedUserSignInLogWriter> _logger;
    private readonly Channel<UserSignInLog> _channel;
    private readonly int _batchSize;

    private int _droppedCount;

    public BufferedUserSignInLogWriter(
        IUserSignInLogService service,
        ILogger<BufferedUserSignInLogWriter> logger,
        int capacity = 10_000,
        int batchSize = 200)
    {
        _service = service;
        _logger = logger;
        _batchSize = batchSize;
        _channel = Channel.CreateBounded<UserSignInLog>(new BoundedChannelOptions(capacity)
        {
            // Keep the newest rows: under a flood, the recent attempts are the ones worth having.
            FullMode = BoundedChannelFullMode.DropOldest,
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

        // DropOldest means TryWrite still succeeds after evicting, so the queue depth staying flat
        // across the write is the only signal that something was discarded.
        var before = _channel.Reader.Count;

        if (!_channel.Writer.TryWrite(record))
        {
            Interlocked.Increment(ref _droppedCount);
            return;
        }

        if (before > 0 && _channel.Reader.Count <= before)
        {
            var dropped = Interlocked.Increment(ref _droppedCount);
            _logger.LogWarning("Sign-in audit buffer full; dropped {DroppedCount} record(s) so far.", dropped);
        }
    }

    public virtual async Task FlushAsync(CancellationToken cancellationToken)
    {
        var batch = new List<UserSignInLog>(_batchSize);

        while (_channel.Reader.TryRead(out var record))
        {
            batch.Add(record);

            if (batch.Count >= _batchSize)
            {
                await PersistAsync(batch);
                batch = new List<UserSignInLog>(_batchSize);
            }
        }

        if (batch.Count > 0)
        {
            await PersistAsync(batch);
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

            await FlushAsync(stoppingToken);
        }

        // Drain what is left so a graceful shutdown does not lose the tail of the trail.
        await FlushAsync(CancellationToken.None);
    }

    private async Task PersistAsync(List<UserSignInLog> batch)
    {
        try
        {
            await _service.SaveChangesAsync(batch);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist {Count} sign-in audit record(s).", batch.Count);
        }
    }
}
