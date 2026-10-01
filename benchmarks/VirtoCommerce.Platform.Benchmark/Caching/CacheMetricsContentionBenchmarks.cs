using System;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace VirtoCommerce.Platform.Benchmark.Caching;

// Unlike Parallel.For, these workers guarantee eight distinct live threads for every invocation.
// Barriers are amortized over 32,768 operations. Thread startup and ExecutionContext capture are setup
// work, so all workers share the same request accumulator in the Request state.
[MemoryDiagnoser]
[ThreadingDiagnoser]
public class CacheMetricsContentionBenchmarks
{
    private const int WorkerCount = 8;
    private const int ReadsPerWorker = 4096;
    private CacheMetricsBenchmarks _scenario;
    private EightWorkers _workers;
    private Action _keys;
    private Action _lookups;

    [Params("Off", "Meter", "Request")]
    public string State { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _scenario = new CacheMetricsBenchmarks { State = State };
        _scenario.Setup();
        _workers = new EightWorkers();
        _keys = () =>
        {
            for (var i = 0; i < ReadsPerWorker; i++)
            {
                GC.KeepAlive(_scenario.TypedKey());
            }
        };
        _lookups = () =>
        {
            for (var i = 0; i < ReadsPerWorker; i++)
            {
                if (!_scenario.TypedHit())
                {
                    throw new InvalidOperationException("Expected a hit");
                }
            }
        };
        _workers.Run(_lookups);
    }

    [Benchmark(OperationsPerInvoke = WorkerCount * ReadsPerWorker)]
    public void TypedKey() => _workers.Run(_keys);

    [Benchmark(OperationsPerInvoke = WorkerCount * ReadsPerWorker)]
    public void TypedHitInOneRequest() => _workers.Run(_lookups);

    [GlobalCleanup]
    public void Cleanup()
    {
        _workers.Dispose();
        _scenario.Cleanup();
    }

    private sealed class EightWorkers : IDisposable
    {
        private readonly Barrier _start = new(WorkerCount + 1);
        private readonly Barrier _done = new(WorkerCount + 1);
        private readonly Thread[] _threads = new Thread[WorkerCount];
        private Action _action;
        private bool _stop;

        public EightWorkers()
        {
            for (var index = 0; index < WorkerCount; index++)
            {
                _threads[index] = new Thread(Work) { IsBackground = true };
                _threads[index].Start();
            }
        }

        public void Run(Action action)
        {
            _action = action;
            _start.SignalAndWait();
            _done.SignalAndWait();
        }

        private void Work()
        {
            while (true)
            {
                _start.SignalAndWait();
                if (_stop)
                {
                    return;
                }
                _action();
                _done.SignalAndWait();
            }
        }

        public void Dispose()
        {
            _stop = true;
            _start.SignalAndWait();
            foreach (var thread in _threads)
            {
                thread.Join();
            }
            _start.Dispose();
            _done.Dispose();
        }
    }
}
