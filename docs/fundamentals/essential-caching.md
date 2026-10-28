>There are only two hard things in Computer Science: cache invalidation and naming things.
-- Phil Karlton

Caching is one of the most effective ways to improve website performance. VirtoCommerce has tried a few different ways to cache application data to reduce the load on external services and database, and minimize application latency when handling API requests. In this article, we describe the technical details and the best caching practices we employ in our platform.

## Cache-Aside pattern overview
We chose [Cache-Aside](https://docs.microsoft.com/en-us/azure/architecture/patterns/cache-aside) as the main pattern for all caching logic, because it is very simple and straightforward for implementation and testing.

The pattern enables applications to load data on demand:

![image](../media/essential-caching-1.png) 

When we need specific data, we first try to get it from the cache. If the data is not in the cache, we get it from the source, add it to the cache and return it. Next time, this data will be returned from the cache. This pattern improves performance and also helps maintain consistency between data held in the cache and data in the underlying data storage.

## Challenges
We don't use the distributed cache in the platform code, because we want to keep the platform configuration flexible and simple, and prefer to solve potential scalability problems by other means (see *Scalability* below).

There are three additional cons of using distributed cache that influenced our decision:

- All cached data must support serialization and deserialization; it is not always possible with distributed cache.
- Decreased performance in comparison to memory cache due to network calls for cached data.
- Increased mixed mode (memory and distributed) complexity.

For platform cache we experimented with the [IMemoryCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1) that stores cached data in the memory.

A simple `Cache-Aside` pattern implementation using `IMemoryCache` looks like this:

```C#
public object GetDataById(string objectId)
{
    object data;
    if (!this._memoryCache.TryGetValue($"cache-key-{objectId}", out data))
    {
        data = this.GetObjectFromDatabase(objectId);
        this._memoryCache.Set($"cache-key-{objectId}", data, new TimeSpan(0, 5, 0));
    }
    return data;
}
```

This code has a few disadvantages:

 - It contains too many lines of code and must be simplified.
 - It requires manual creation of the cache key that cannot guarantee its uniqueness.
 - It does not protect against race conditions, when multiple streams will try to access the same cache key simultaneously, which may lead to excess data eviction. This may not be a problem, unless your application has a high concurrent load and costly backend requests, or the backend is not designed to handle simultaneous requests.
 - It supposes manual control of the cached data lifetime. Chosing proper values for the lifetime is complicated and reduces developer's productivity.
  
The relatively new `MemoryCache` methods `GetOrCreate/GetOrCreateAsync` also suffer from these problems, which means we can't use them as they are, too. This article describes the issue in greater detail: [ASP.NET Core Memory Cache - Is the GetOrCreate method thread-safe](https://blog.novanet.no/asp-net-core-memory-cache-is-get-or-create-thread-safe/).

## Solution

To solve the aforementioned issues, we defined our own [IMemoryCacheExtensions](https://github.com/VirtoCommerce/vc-platform/blob/master/src/VirtoCommerce.Platform.Core/Caching/MemoryCacheExtensions.cs). This implementation guarantees that the cacheable delegates (cache misses) are only run once without race conditions. Also, this extension provides more compact syntax for the client code.

Let's rewrite the previous example with the new extension:

```C#
1   public object GetDataById(string objectId)
2   {
3       object data;
4       var cacheKey = CacheKey.With(GetType(), nameof(GetDataById), id);
5       var data = _memoryCache.GetOrCreateExclusive(cacheKey, cacheEntry =>
6           {
7             cacheEntry.AddExpirationToken(MyCacheRegion.CreateChangeToken()); 
8             return this.GetObjectFromDatabase(objectId);
9           });
10      return data;
11  }
```

### Cache keys generation

A special static class `CacheKey` (line `4`) provides a method for unique string cache key generation according to the arguments and type/method information passed.

E.g:

```C#

 CacheKey.With(GetType(), nameof(GetDataById), "123"); /* => "TypeName:GetDataById-123" */

```

`CacheKey` can also be used to generate cache keys for complex types objects. Most of the platform types are derived from `Entity` or `ValueObject` classes, each of these types implement the `ICacheKey` interface that contains `GetCacheKey()` method which can be used for cache key generation. 

In the following code, we create a cache key for a complex type object:

```C#
class ComplexValueObject : ValueObject
{
    public string Prop1 { get; set; }
    public string Prop2 { get; set; }
}

var valueObj = new ComplexValueObject { Prop1 = "Prop1Value", Prop2 = "Prop2Value" };
var data = CacheKey.With(valueObj.GetCacheKey());
//cacheKey will take the value "Prop1Value-Prop2Value"

```

### Thread-safe caching and avoiding race conditions

In line `5`, the `_memoryCache.GetOrCreateExclusive()` method calls a thread-safe caching extension that guarantees that the cacheable delegate (cache miss) only executes once in multiple threads race.

An asynchronous version of this extension method is also available:  `_memoryCache.GetOrCreateExclusiveAsync()`.

The following code demonstrates how this exclusive access to the cacheable delegate work:

```C#
        public void GetOrCreateExclusive()
        {
            var sut = new MemoryCache();
            int counter = 0;
            Parallel.ForEach(Enumerable.Range(1, 10), i =>
            {
                var item = sut.GetOrCreateExclusive("test-key", cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(10);
                    return Interlocked.Increment(ref counter);
                });
               Console.Write($"{item} ");
            });
        }
```

**Output**

```Console
1 1 1 1 1 1 1 1 1 1
```

### Cache expiration and eviction

In line `7`, a `CancellationTokenSource` object is created. It is associated with the cache data and a strongly typed cache region, which allows multiple cache entries to be evicted as a group (see [ASP.NET Core Memory Cache dependencies](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1#cache-dependencies)).

> Important: We intentionally disable the inheritance for cached entries expiration tokens and time-based expiration settings. When one cache entry is used to create another, the child copies the parent entry's expiration settings and cannot be expired by manual removal or updating of the parent entry. This leads to unpredictable side-effects, and it is hard to maintain and debug such cache dependencies.

 We avoid manual control of the cached data lifetime in our code. The platform has a special `CachingOptions` object that contains the settings for Absolute **or** Sliding lifetimes for all cached data (see below). 

 Thanks to the `Clean Architecture` and the `Bounded contexts`, where each boundary controls all read/change operations for data belonging to the domain, we can always keep the cache in actual state and evict modified data from it explicitly.

  
### Strongly typed cache regions

The platform supports a construct called strongly typed cache regions that is used to control a set of cache keys and provides the tools to evict from cache grouped/related data to keep cache consistent. To define our own cache region, we need to derive it from `CancellableCacheRegion<>`. Then the `ExpireRegion` method can be used to remove all keys within one region: 

```C#

//Region definition
public static class MyCacheRegion : CancellableCacheRegion<MyCacheRegion>
{    
}

//Usage
cacheEntry.AddExpirationToken(MyCacheRegion.CreateChangeToken()); 

//Expire all data associated with the region
MyCacheRegion.ExpireRegion();

```

There also is the special `GlobalCacheRegion` that can be used to expire all cached data of the entire application:

```C#
//Expire all cached data for entire application
GlobalCacheRegion.ExpireRegion();
```

### Caching null values

By default, the platform caches `null` values. If `negative caching` is the design choice, this default behavior can be changed by passing `false` to `cacheNullValue` in the `GetOrCreateExclusive` method, e.g.:

```C#

 var data = _memoryCache.GetOrCreateExclusive(cacheKey, cacheEntry => {}, cacheNullValue: false);

```

## Cache settings

The default platform caching options can be changed from configuration:

*appsettings.json*
```json
 "Caching": {
        //Set to false to disable caching of application data for the entire application
        "CacheEnabled": true, 
        //Sets a sliding expiration time for all application cached data that doesn't have an expiration value set manually
        "CacheSlidingExpiration": "0:15:00", 
        //Sets an absolute expiration time for all cached data that doesn't have an expiration value set manually
        //"CacheAbsoluteExpiration": "0:15:00"
    }
```

## Cache hit/miss metrics

The `VirtoCommerce.Platform.Caching` meter publishes two counters. Subscribe through the deployment's
existing OpenTelemetry configuration; the platform does not depend on an Application Insights SDK.

| Instrument | Population | Dimensions |
| --- | --- | --- |
| `virtocommerce.cache.requests` | Physical lookups, including internal rechecks; independent of trace sampling | `cache.request.type` = `hit` / `miss`, `cache.name` |
| `virtocommerce.cache.request.groups` | One observation per retained cache group at completion of a sampled HTTP request | `cache.name`, `cache.outcome` = `hit_only` / `mixed` / `miss_only` |

Neither instrument adds request IDs, trace IDs, entity IDs or full cache keys as dimensions.
Listener failures are isolated from cache reads and request completion; a failing listener can lose
telemetry but cannot fail a load or leave a request-scoped reservation incomplete.
This includes instrument publication on first use: a counter whose publication fails remains disabled
for that process. Other counters and sampled request attributes can still operate.

### How `cache.name` is selected

`CacheKey.With(Type, ...)` registers a stable type-owned prefix. Lookups resolve that registered prefix
without allocating a substring. Existing key identity, case normalization and invalidation do not change.

- Generic CRUD and search services associate their runtime owner type with `TModel`, producing names
  such as `CatalogProduct`, `Category` and `InventoryInfo`.
- Other registered owners use their type name, such as `SettingsManager`.
- Unclassified platform-memory keys use `PlatformMemoryCache`. Both request-scoped APIs use
  `RequestScopedCache` for an unregistered prefix, even when the caller embeds request data in it.
- Type names omit namespaces. The first model registration labels the shared short prefix. At the
  first conflicting model registration, that label changes once to the owner prefix and stays there;
  later service resolutions cannot switch it back. Before that conflict is discovered, resolution
  order determines which model name is visible. An owner with the same short name that never registers
  a model shares the existing mapping; the string key cannot distinguish those owner types.

For example, `ProductService:GetAsync-Full-product123` is classified as `CatalogProduct` after
`ProductService` registers that model. The method, response group and product ID never become tags.
No per-key metadata is retained. Registration and model association are cached; repeated transient
service construction does not rewrite the registry.

Custom services can call `CacheKey.RegisterCacheName(GetType(), typeof(MyModel))` during construction
and build their keys through `CacheKey.With(GetType(), ...)`. This also works for modules compiled
against the existing key API. `IRequestScopedCache.GetOrAddAsync(key, factory)` remains the by-key
interface and virtual extension point; no named overload is required.

### Counting semantics

These counters do not describe a logical entity hit rate, HTTP request count, factory invocations,
saved database calls, freshness or elapsed time.

| Operation | Physical observations |
| --- | --- |
| `PlatformMemoryCache.TryGetValue` (including Redis backplane variant) | One lookup; Redis does not add another observation |
| Cold `GetOrCreateExclusive` / `GetOrCreateExclusiveAsync` | Two misses, including the recheck under the lock |
| Cold `GetOrLoadByIdsAsync`, N distinct nonempty IDs | `3N + 1` misses: initial all-hit probe, N batch probes, two probes per missing ID |
| Warm `GetOrLoadByIdsAsync`, N IDs | N hits |
| `RequestScopedCache.GetOrAddAsync` | One lookup; the reservation winner misses, concurrent followers hit |
| `RequestScopedCache.GetOrLoadMapByIdsAsync` | One lookup per nonempty input ID; duplicate IDs are additional hits; increments are batched per call |

A ten-ID platform batch with nine cached entities records 9 hits / 4 misses if the missing ID comes
first (69% physical hits), or 18 hits / 4 misses if it comes last (82%). Neither is the logical 90%
entity hit rate. Do not derive that rate from `virtocommerce.cache.requests`. A group with misses
and **zero hits over an observation window** is a useful signal for keys that never match.

Cached nulls, in-flight request-scoped loads and cached request-scoped failures are hits. Null/empty
IDs are skipped. These semantics include the existing helpers' internal probes, without changing
how the helpers load, deduplicate or cache failures.

Observe locally:

```console
dotnet-counters monitor --process-id <pid> --counters VirtoCommerce.Platform.Caching
```

With the OpenTelemetry module, include the meter in the deployment's existing list:

```json
{
  "OpenTelemetry": {
    "Meters": ["VirtoCommerce.Platform.Caching"]
  }
}
```

### Cache lookups in one HTTP request

For an HTTP server activity with **both `IsAllDataRequested` and `Recorded` set**, whose
**`ActivitySource` has a nonempty name and a listener**, a request with at least one lookup receives:

- `cache.hits` and `cache.misses`: numeric totals across both cache implementations.
- `cache.lookup.summary`: compact JSON arrays `[cacheName, hits, misses]`, for example
  `[["CatalogProduct",28,0],["Category",3,1]]`.
- `cache.groups.truncated=true` only if a detail limit was reached.

In Aspire, select the HTTP server span under **Traces** and inspect these attributes. No per-group
`ActivityEvent` is emitted. This avoids the Azure Monitor exporter's separate `traces` telemetry
item for every such event, while keeping per-group counts on the original request.

Lookups inside awaited child activities and parallel work belong to the HTTP server activity,
obtained through `IHttpActivityFeature`, rather than whichever child happens to be current.
Concurrent requests remain isolated. Completion also runs on downstream failure. It reads each
counter once and reuses those values for totals, group detail and outcomes. Lookups completed before
request completion are included; detached lookups still in flight while that snapshot is read may
be omitted. Completion does not wait for detached work, which cannot change the published snapshot.
Background lookups still contribute to the aggregate physical counter when subscribed.

There is no summary for an absent activity, a legacy activity with the default empty-name source (even
if that source has a listener and an incoming `traceparent` sets its `Recorded` flag), a `RecordOnly`
sampling result, or a request without lookups. Request attribution requires a tracing listener, but
does not require a meter listener.
An unexported trace cannot be inspected.

Detail is bounded to the first 64 admitted groups and 8 KiB of UTF-8 JSON. Truncation never removes
lookups from overall totals. An exporter can impose a smaller attribute budget; configure it to
retain the summary if using this drill-down. A storefront action may issue several HTTP requests,
each with its own summary.

### Application Insights SDK 3.x / Azure Monitor

The Azure Monitor exporter stores these span attributes as **strings in `customDimensions`**, not
`customMeasurements`. Convert them in KQL when inspecting an individual request:

```kusto
requests
| where id == "<request-span-id>"
| extend cacheHits = tolong(customDimensions["cache.hits"]),
         cacheMisses = tolong(customDimensions["cache.misses"]),
         groups = parse_json(tostring(customDimensions["cache.lookup.summary"]))
| mv-expand group = groups
| project timestamp, operation_Id, id,
          cacheName = tostring(group[0]), hits = tolong(group[1]), misses = tolong(group[2]),
          cacheHits, cacheMisses,
          truncated = tobool(customDimensions["cache.groups.truncated"])
```

For an aggregate signal such as “in how many sampled requests did this cache never hit”, use the
request-group counter. It emits once per retained group, at completion, including failed requests.
Its maximum is 64 group observations per sampled request; groups beyond that bound are omitted.
The JSON byte budget does not further limit the metric's admitted groups.

```kusto
customMetrics
| where name == "virtocommerce.cache.request.groups"
| extend cacheName = tostring(customDimensions["cache.name"]),
         outcome = tostring(customDimensions["cache.outcome"])
| summarize hitOnly = sumif(valueSum, outcome == "hit_only"),
            mixed = sumif(valueSum, outcome == "mixed"),
            missOnly = sumif(valueSum, outcome == "miss_only") by cacheName
| extend missOnlyRatio = todouble(missOnly) / (hitOnly + mixed + missOnly)
```

This is a **sampled population** signal: compare outcome proportions with the sampling policy and
truncation in mind. It is not an exact all-traffic request count or an entity hit rate. The physical
lookup counter remains independent of sampling. Exporter configuration and delivery still determine
which telemetry reaches Azure.

## Scaling

Running multiple instances of the platform, all accessing the local cache that must be consistent with cache of other instances, can be tricky. [How to scale out platform on Azure](../techniques/how-scale-out-platform-on-azure.md) explains how to configure `Redis` service as a cache backplane to sync local caches for multiple platform instances.

## Conclusions

- The [IMemoryCacheExtensions](https://github.com/VirtoCommerce/vc-platform/blob/master/src/VirtoCommerce.Platform.Core/Caching/MemoryCacheExtensions.cs) extension contains sync and async extension methods that represent the compact form of `Cache-Aside` pattern implementation on the `ASP.NET Core IMemoryCache` interface and provide exclusive access to the original data in a race condition.
- In order to avoid issues with stale cached data, always keep your cached data in consistent state using the strongly typed `cache regions` that allow evicting groups of data.
- The platform uses an aggressive caching policy for most DAL services, even caching large search results. Do not use relative size metrics for cached data, as it may lead to high memory utilization in some production scenarios. Play with `CacheSlidingExpiration`/`CacheAbsoluteExpiration` values to find an optimal balance of memory consumption and application performance.
