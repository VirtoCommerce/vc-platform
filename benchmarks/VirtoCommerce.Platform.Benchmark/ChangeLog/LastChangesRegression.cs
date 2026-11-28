using System;
using System.Collections.Generic;
using System.Threading;
using VirtoCommerce.Platform.Caching;

namespace VirtoCommerce.Platform.Benchmark.ChangeLog;

// Runs the same behavior scenarios against the Legacy and Current implementations and prints a table.
//   dotnet run -c Release -- lastchanges-regression
public static class LastChangesRegression
{
    private sealed record Scenario(string Name, Func<LastChangesSubject, bool> Check);

    public static void Run()
    {
        var scenarios = new List<Scenario>
        {
            new("Repeated read of a name returns the same date", s =>
            {
                var a = s.Service.GetLastModifiedDate("Product");
                LastChangesSubject.WaitForClockTick();
                return a == s.Service.GetLastModifiedDate("Product");
            }),
            new("Reset(name) moves that name's date forward", s =>
            {
                var before = s.Service.GetLastModifiedDate("Order");
                LastChangesSubject.WaitForClockTick();
                s.Service.Reset("Order");
                return s.Service.GetLastModifiedDate("Order") > before;
            }),
            new("Reset(name) does not change other names", s =>
            {
                var other = s.Service.GetLastModifiedDate("Price");
                LastChangesSubject.WaitForClockTick();
                s.Service.Reset("Inventory");
                return s.Service.GetLastModifiedDate("Price") == other;
            }),
            new("Reset is case-insensitive", s =>
            {
                var before = s.Service.GetLastModifiedDate("Category");
                LastChangesSubject.WaitForClockTick();
                s.Service.Reset("CATEGORY");
                return s.Service.GetLastModifiedDate("category") > before;
            }),
            new("Two reads after one reset return the same date", s =>
            {
                s.Service.Reset("Member");
                var a = s.Service.GetLastModifiedDate("Member");
                LastChangesSubject.WaitForClockTick();
                return a == s.Service.GetLastModifiedDate("Member");
            }),
            new("Reset from another instance (Redis) moves the date forward", s =>
            {
                var before = s.Service.GetLastModifiedDate("Store");
                LastChangesSubject.WaitForClockTick();
                s.RemoteReset("Store");
                return s.Service.GetLastModifiedDate("Store") > before;
            }),
            new("Region reset moves every date forward", s =>
            {
                var read = s.Service.GetLastModifiedDate("Catalog");
                s.Service.Reset("Promotion");
                var reset = s.Service.GetLastModifiedDate("Promotion");
                LastChangesSubject.WaitForClockTick();
                s.ExpireRegion();
                return s.Service.GetLastModifiedDate("Catalog") > read && s.Service.GetLastModifiedDate("Promotion") > reset;
            }),
            new("Global cache reset moves every date forward", s =>
            {
                var before = s.Service.GetLastModifiedDate("Pricelist");
                LastChangesSubject.WaitForClockTick();
                GlobalCacheRegion.ExpireRegion();
                return s.Service.GetLastModifiedDate("Pricelist") > before;
            }),
            new("Date of a reset name is newer than a read before it", s =>
            {
                var unknown = s.Service.GetLastModifiedDate("Contact");
                LastChangesSubject.WaitForClockTick();
                s.Service.Reset("Organization");
                return s.Service.GetLastModifiedDate("Organization") > unknown;
            }),
            new("Null name does not throw", s =>
            {
                s.Service.GetLastModifiedDate(null);
                return true;
            }),
            // Expected differences: the legacy behavior is what VCST-7077 removes
            new("[changed] Unchanged name keeps its date after 1 s of cache expiry (no false 'changed')", _ =>
            {
                var s = LastChangesSubject.Create(_.Implementation, TimeSpan.FromMilliseconds(200));
                var before = s.Service.GetLastModifiedDate("Seo");
                Thread.Sleep(1000);
                s.MemoryCache.Compact(1.0);
                return s.Service.GetLastModifiedDate("Seo") == before;
            }),
            new("[changed] 10,000 reads of unknown names add no static tokens", s =>
            {
                var before = s.KeyTokenCount();
                foreach (var name in LastChangesSubject.UniqueNames($"regression-{Guid.NewGuid():N}", 10_000))
                {
                    s.Service.GetLastModifiedDate(name);
                }

                return s.KeyTokenCount() == before;
            }),
        };

        Console.WriteLine($"| Scenario | {LastChangesSubject.Legacy} | {LastChangesSubject.Current} |");
        Console.WriteLine("|---|---|---|");

        foreach (var scenario in scenarios)
        {
            var legacy = Check(scenario, LastChangesSubject.Legacy);
            var current = Check(scenario, LastChangesSubject.Current);
            Console.WriteLine($"| {scenario.Name} | {legacy} | {current} |");
        }
    }

    private static string Check(Scenario scenario, string implementation)
    {
        try
        {
            return scenario.Check(LastChangesSubject.Create(implementation)) ? "pass" : "FAIL";
        }
        catch (Exception ex)
        {
            return $"ERROR {ex.GetType().Name}";
        }
    }
}
