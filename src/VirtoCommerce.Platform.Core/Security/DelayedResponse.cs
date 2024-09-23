using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

// This class allows to measure the duration of a succeeded response and delay subsequent failed responses to prevent timing attacks.
public class DelayedResponse
{
    private static readonly ConcurrentDictionary<string, long> _delaysByName = new();

    private readonly string _name;
    private readonly Stopwatch _stopwatch;
    private readonly Task _delayTask;

    public static DelayedResponse Create(params string[] nameParts)
    {
        return new DelayedResponse(string.Join(".", nameParts));
    }

    public DelayedResponse(string name)
    {
        _name = name;
        _stopwatch = Stopwatch.StartNew();
        _delaysByName.TryAdd(name, 0L);
        _delayTask = Task.Delay((int)_delaysByName[name]);
    }

    public Task FailAsync()
    {
        return _delayTask;
    }

    public Task SucceedAsync()
    {
        if (_stopwatch.IsRunning)
        {
            _stopwatch.Stop();
            _delaysByName[_name] = _stopwatch.ElapsedMilliseconds;
        }

        return Task.CompletedTask;
    }
}
