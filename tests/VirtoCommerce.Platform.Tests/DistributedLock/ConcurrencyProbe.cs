using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Tests.DistributedLock;

/// <summary>
/// Records how many callers are inside a critical section at the same time.
/// </summary>
internal sealed class ConcurrencyProbe
{
    private int _inside;
    private int _maxConcurrency;
    private int _completed;

    public int MaxConcurrency => Volatile.Read(ref _maxConcurrency);

    public int Completed => Volatile.Read(ref _completed);

    public async Task EnterAsync(TimeSpan hold, CancellationToken cancellationToken)
    {
        var current = Interlocked.Increment(ref _inside);
        var observed = Volatile.Read(ref _maxConcurrency);
        while (observed < current)
        {
            var previous = Interlocked.CompareExchange(ref _maxConcurrency, current, observed);
            if (previous == observed)
            {
                break;
            }

            observed = previous;
        }

        try
        {
            await Task.Delay(hold, cancellationToken);
        }
        finally
        {
            Interlocked.Decrement(ref _inside);
            Interlocked.Increment(ref _completed);
        }
    }
}
