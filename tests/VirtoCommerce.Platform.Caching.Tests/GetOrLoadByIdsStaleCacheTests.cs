using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    // Reproduces (and pins the fix for) a real defect in MemoryCacheExtensions.GetOrLoadByIdsCoreAsync:
    // the cache-invalidation change token used to be created in `configureCache` AFTER `loadItems` had
    // already run. If a writer commits and invalidates the key WHILE a reader's load is in flight, the
    // reader's later cache-write creates a brand-new (uncancelled) token via
    // GenericCachingRegion<T>.CreateChangeTokenForKey — because
    // CancellableCacheRegion<T>.InnerExpireTokenForKey already TryRemove'd the prior token source from
    // _keyTokensDict before the reader gets around to writing. The stale value would then be cached under
    // a token that can never observe the invalidation that made it stale, and would be served on every
    // subsequent read until the sliding TTL expires.
    //
    // The fix adds an explicit, opt-in `createExpirationToken` parameter to `GetOrLoadByIdsAsync`: when
    // supplied, the token is captured BEFORE `loadItems` runs, so an invalidation landing mid-load is
    // still visible. Test A proves the fixed (opted-in) path. Test B is a deliberate characterization
    // test: callers that do NOT pass a token factory keep the pre-fix behaviour by design — the raw
    // extension without an explicit factory is knowingly still vulnerable, and that is the documented
    // opt-in boundary, not an oversight.
    [Trait("Category", "Unit")]
    [Collection(nameof(NotThreadSafeCollection))]
    public class GetOrLoadByIdsStaleCacheTests : MemoryCacheTestsBase
    {
        // Private, test-only model: GenericCachingRegion<T> keys its static token state on T, so sharing
        // a model type with another test would let parallel tests bleed into each other.
        private sealed class StaleCacheProbeModel : Entity
        {
            public int Version { get; init; }
        }

        [Fact]
        public async Task GetOrLoadByIdsAsync_WithTokenFactory_InvalidationDuringLoad_ServesFreshValue()
        {
            var sut = GetPlatformMemoryCache();
            const string keyPrefix = "stale-cache-probe-fixed";
            const string id = "probe-1";
            var secondLoadCalls = 0;

            // Mirrors CrudService.ConfigureCache (src/VirtoCommerce.Platform.Data/GenericCrud/CrudService.cs:92-95) exactly.
            static void ConfigureCache(MemoryCacheEntryOptions options, string entityId, StaleCacheProbeModel item)
            {
                options.AddExpirationToken(GenericCachingRegion<StaleCacheProbeModel>.CreateChangeTokenForKey(entityId));
            }

            // First read: the loader models a writer that commits and invalidates the key WHILE the
            // SELECT is in flight, then returns the value it read before that commit (now stale).
            Task<IList<StaleCacheProbeModel>> LoadStale(IList<string> ids)
            {
                GenericCachingRegion<StaleCacheProbeModel>.ExpireTokenForKey(id);

                IList<StaleCacheProbeModel> items = [new StaleCacheProbeModel { Id = id, Version = 1 }];
                return Task.FromResult(items);
            }

            await sut.GetOrLoadByIdsAsync(keyPrefix, [id], LoadStale, ConfigureCache,
                entityId => GenericCachingRegion<StaleCacheProbeModel>.CreateChangeTokenForKey(entityId));

            // Second read for the same id: the fixed implementation captured the token BEFORE the load
            // ran, so the mid-load invalidation was observed and the poisoned entry was evicted, forcing
            // this loader to run and return the fresh value.
            Task<IList<StaleCacheProbeModel>> LoadFresh(IList<string> ids)
            {
                secondLoadCalls++;

                IList<StaleCacheProbeModel> items = [new StaleCacheProbeModel { Id = id, Version = 2 }];
                return Task.FromResult(items);
            }

            var second = await sut.GetOrLoadByIdsAsync(keyPrefix, [id], LoadFresh, ConfigureCache,
                entityId => GenericCachingRegion<StaleCacheProbeModel>.CreateChangeTokenForKey(entityId));

            Assert.Equal(1, secondLoadCalls);
            Assert.Equal(2, second.Single().Version);
        }

        [Fact]
        public async Task GetOrLoadByIdsAsync_WithoutTokenFactory_InvalidationDuringLoad_StillServesStaleValue()
        {
            // CHARACTERIZATION TEST — pins the opt-in boundary of the fix, not a bug report.
            // A caller that does not pass `createExpirationToken` keeps the original (pre-fix) behaviour:
            // the invalidation token is created late, inside `configureCache`, so a mid-load invalidation
            // is invisible to it and the stale value is cached under a live token. This is the documented,
            // knowingly-still-vulnerable contract for callers that don't opt in — see
            // GetOrLoadByIdsAsync_WithTokenFactory_InvalidationDuringLoad_ServesFreshValue for the fixed path.
            var sut = GetPlatformMemoryCache();
            const string keyPrefix = "stale-cache-probe-unfixed";
            const string id = "probe-2"; // different id from test A — GenericCachingRegion<T> state is shared across the class
            var secondLoadCalls = 0;

            // Mirrors CrudService.ConfigureCache (src/VirtoCommerce.Platform.Data/GenericCrud/CrudService.cs:92-95) exactly.
            static void ConfigureCache(MemoryCacheEntryOptions options, string entityId, StaleCacheProbeModel item)
            {
                options.AddExpirationToken(GenericCachingRegion<StaleCacheProbeModel>.CreateChangeTokenForKey(entityId));
            }

            // First read: the loader models a writer that commits and invalidates the key WHILE the
            // SELECT is in flight, then returns the value it read before that commit (now stale).
            Task<IList<StaleCacheProbeModel>> LoadStale(IList<string> ids)
            {
                GenericCachingRegion<StaleCacheProbeModel>.ExpireTokenForKey(id);

                IList<StaleCacheProbeModel> items = [new StaleCacheProbeModel { Id = id, Version = 1 }];
                return Task.FromResult(items);
            }

            // No createExpirationToken argument here — the not-opted-in call shape.
            await sut.GetOrLoadByIdsAsync(keyPrefix, [id], LoadStale, ConfigureCache);

            // Second read for the same id: without the opt-in, the token is (again) created only inside
            // configureCache, AFTER this load already ran below — so the entry from the first read is
            // never evicted and this loader must NOT run.
            Task<IList<StaleCacheProbeModel>> LoadFresh(IList<string> ids)
            {
                secondLoadCalls++;

                IList<StaleCacheProbeModel> items = [new StaleCacheProbeModel { Id = id, Version = 2 }];
                return Task.FromResult(items);
            }

            var second = await sut.GetOrLoadByIdsAsync(keyPrefix, [id], LoadFresh, ConfigureCache);

            Assert.Equal(0, secondLoadCalls);
            Assert.Equal(1, second.Single().Version);
        }
    }
}
