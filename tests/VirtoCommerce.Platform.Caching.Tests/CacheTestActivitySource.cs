using System;
using System.Diagnostics;

namespace VirtoCommerce.Platform.Caching.Tests;

internal sealed class CacheTestActivitySource : IDisposable
{
    private readonly ActivitySource _source = new("CacheMetrics.Tests");
    private readonly ActivityListener _listener;

    public CacheTestActivitySource()
    {
        _listener = new ActivityListener
        {
            ShouldListenTo = source => ReferenceEquals(source, _source),
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
        };
        ActivitySource.AddActivityListener(_listener);
    }

    public Activity Start(string name) => _source.StartActivity(name, ActivityKind.Server);

    public void Dispose()
    {
        _listener.Dispose();
        _source.Dispose();
    }
}
