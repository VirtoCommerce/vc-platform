using System;
using BenchmarkDotNet.Attributes;

namespace VirtoCommerce.Platform.Benchmark.ChangeLog;

// CPU and allocations of ILastChangesService before (Legacy) and after (Current) VCST-7077.
//   dotnet run -c Release -- --filter '*LastChangesBenchmarks*'
[MemoryDiagnoser]
public class LastChangesBenchmarks
{
    private const int _knownCount = 50;
    private const int _uniqueCount = 10_000;

    private LastChangesSubject _subject;
    private string[] _knownNames;
    private string[] _uniqueNames;

    [Params(LastChangesSubject.Legacy, LastChangesSubject.Current)]
    public string Implementation { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _subject = LastChangesSubject.Create(Implementation);
        _knownNames = LastChangesSubject.UniqueNames("VirtoCommerce.Module.Model.Entity", _knownCount);

        // Half of the names were saved at least once, half were only read
        for (var i = 0; i < _knownNames.Length; i++)
        {
            if (i % 2 == 0)
            {
                _subject.Service.Reset(_knownNames[i]);
            }

            _subject.Service.GetLastModifiedDate(_knownNames[i]);
        }
    }

    // A client polling the same entity type names (the normal changed-entities call)
    [Benchmark(OperationsPerInvoke = _knownCount)]
    public DateTimeOffset PollKnownNames()
    {
        var result = default(DateTimeOffset);
        foreach (var name in _knownNames)
        {
            result = _subject.Service.GetLastModifiedDate(name);
        }

        return result;
    }

    // An entity save (LastChangesNotifier) followed by a poll of the same name
    [Benchmark(OperationsPerInvoke = _knownCount)]
    public DateTimeOffset ResetThenRead()
    {
        var result = default(DateTimeOffset);
        foreach (var name in _knownNames)
        {
            _subject.Service.Reset(name);
            result = _subject.Service.GetLastModifiedDate(name);
        }

        return result;
    }

    [IterationSetup(Target = nameof(ReadUniqueNames))]
    public void CreateUniqueNames()
    {
        _uniqueNames = LastChangesSubject.UniqueNames($"unique-{Guid.NewGuid():N}", _uniqueCount);
    }

    [IterationCleanup(Target = nameof(ReadUniqueNames))]
    public void ClearUniqueNames()
    {
        // Keeps the legacy registry from growing across iterations; the retained-memory run measures that growth
        _subject.ExpireRegion();
        _subject.MemoryCache.Compact(1.0);
    }

    // The PLATFORM-04 request: names the platform never saw
    [Benchmark(OperationsPerInvoke = _uniqueCount)]
    public DateTimeOffset ReadUniqueNames()
    {
        var result = default(DateTimeOffset);
        foreach (var name in _uniqueNames)
        {
            result = _subject.Service.GetLastModifiedDate(name);
        }

        return result;
    }
}
