using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

// This class allows to measure the duration of a succeeded response and delay subsequent failed responses to prevent timing attacks.
public class DelayedResponse
{
    private const int _minDelay = 150; // milliseconds
    private static readonly ConcurrentDictionary<string, int> _delaysByName = new();

    private readonly string _name;
    private readonly Stopwatch _stopwatch;
    private readonly Task _failedDelayTask;
    private readonly Task _succeededDelayTask;

    public static DelayedResponse Create(params string[] nameParts)
    {
        return new DelayedResponse(string.Join(".", nameParts));
    }

    public DelayedResponse(string name)
    {
        _name = name;
        _stopwatch = Stopwatch.StartNew();
        _delaysByName.TryAdd(name, 0);
        var failedDelay = Math.Max(_minDelay, _delaysByName[name]);
        _failedDelayTask = Task.Delay(failedDelay);
        _succeededDelayTask = Task.Delay(_minDelay);
    }

    public Task FailAsync()
    {
        return _failedDelayTask;
    }

    public Task SucceedAsync()
    {
        if (_stopwatch.IsRunning)
        {
            _stopwatch.Stop();
            _delaysByName[_name] = (int)_stopwatch.ElapsedMilliseconds;
        }

        return _succeededDelayTask;
    }
}
