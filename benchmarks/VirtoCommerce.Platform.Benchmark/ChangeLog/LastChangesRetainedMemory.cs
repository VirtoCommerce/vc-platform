using System;
using System.Diagnostics;

namespace VirtoCommerce.Platform.Benchmark.ChangeLog;

// Simulates the PLATFORM-04 attack in-process: N reads of unique names, as the changed-entities endpoint does.
// Run each implementation in its own process so static state does not mix:
//   dotnet run -c Release -- lastchanges-retained Legacy 1000000
//   dotnet run -c Release -- lastchanges-retained Current 1000000
public static class LastChangesRetainedMemory
{
    public static void Run(string implementation, int count)
    {
        var subject = LastChangesSubject.Create(implementation);

        // Warm up JIT and the type initializers outside the measurement
        foreach (var name in LastChangesSubject.UniqueNames("warmup", 1000))
        {
            subject.Service.GetLastModifiedDate(name);
        }

        subject.ExpireRegion();
        subject.MemoryCache.Compact(1.0);

        var names = LastChangesSubject.UniqueNames("attack", count);
        var baseline = Managed();

        var process = Process.GetCurrentProcess();
        var cpuBefore = process.TotalProcessorTime;
        var allocatedBefore = GC.GetAllocatedBytesForCurrentThread();
        var stopwatch = Stopwatch.StartNew();

        foreach (var name in names)
        {
            subject.Service.GetLastModifiedDate(name);
        }

        stopwatch.Stop();
        process.Refresh();
        var cpu = process.TotalProcessorTime - cpuBefore;
        var allocated = GC.GetAllocatedBytesForCurrentThread() - allocatedBefore;
        var retainedAfterReads = Managed() - baseline;
        var cacheEntries = subject.MemoryCache.Count;
        var keyTokens = subject.KeyTokenCount();

        // What normal cache expiry does after 15 minutes without reads
        subject.MemoryCache.Compact(1.0);
        var retainedAfterEviction = Managed() - baseline;
        var keyTokensAfterEviction = subject.KeyTokenCount();

        GC.KeepAlive(names);

        Console.WriteLine($"| {implementation} | {count:N0} | {stopwatch.Elapsed.TotalMilliseconds:N0} ms | {cpu.TotalMilliseconds:N0} ms | {allocated / 1024.0 / 1024.0:N1} MB | {retainedAfterReads / 1024.0 / 1024.0:N1} MB | {cacheEntries:N0} | {keyTokens:N0} | {retainedAfterEviction / 1024.0 / 1024.0:N1} MB | {keyTokensAfterEviction:N0} |");
    }

    private static long Managed()
    {
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
        GC.WaitForPendingFinalizers();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);

        return GC.GetTotalMemory(forceFullCollection: true);
    }
}
