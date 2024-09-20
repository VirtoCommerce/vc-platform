using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

// This class allows to measure the duration of successful sign in and delay the failed response to prevent timing attacks.
public class SecurityStopwatch
{
    private static readonly ConcurrentDictionary<string, long> _delaysByName = new();

    public static SecurityStopwatch StartNew(params string[] nameParts)
    {
        return new SecurityStopwatch(string.Join(".", nameParts));
    }

    private bool _isRunning;
    private readonly string _name;
    private readonly Stopwatch _stopwatch;
    private readonly Task _delayTask;

    private SecurityStopwatch(string name)
    {
        _isRunning = true;
        _name = name;
        _stopwatch = Stopwatch.StartNew();
        _delaysByName.TryAdd(name, 0L);
        _delayTask = Task.Delay((int)_delaysByName[name]);
    }

    public Task DelayAsync()
    {
        return _delayTask;
    }

    public void Stop()
    {
        if (_isRunning)
        {
            _stopwatch.Stop();
            _delaysByName[_name] = _stopwatch.ElapsedMilliseconds;
            _isRunning = false;
        }
    }
}
