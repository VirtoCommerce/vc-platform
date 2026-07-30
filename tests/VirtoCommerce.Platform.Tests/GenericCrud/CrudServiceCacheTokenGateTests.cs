using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;
using VirtoCommerce.Platform.Tests.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    // Pins the ConfigureCache gate in CrudService.GetAsync. Capturing the invalidation token before the load
    // (VCST-5303) hands CreateCacheToken to the cache helper — and that is a WRITE, not a lookup:
    // GenericCachingRegion<TModel>.CreateChangeTokenForKey does _keyTokensDict.GetOrAdd(...) into
    // process-static state whose only removal path is an explicit expire.
    //
    // For a service that still mints through the default ConfigureCache the early capture is free: the same
    // key set is minted either way, so GetOrAdd returns the source ConfigureCache would have created. For a
    // service that overrides ConfigureCache and redirects the mint to its own region (vc-module-inventory
    // InventoryServiceImpl), the early capture instead accumulates one permanently-live
    // CancellationTokenSource per id in a region that service never expires.
    //
    // Overriding ClearCache does NOT move the mint, so it must not close the gate — vc-module-order
    // CustomerOrderService overrides ClearCache to expire an extra key in the very same region, and it is the
    // service whose stale cached reads VCST-5303 was measured on.
    //
    // Every probe type is generic over a marker so each test gets its own GenericCachingRegion<TModel> static
    // state. Sharing a model type across tests — or with CrudServiceTests, which populates
    // GenericCachingRegion<TestModel> — would let one test's tokens satisfy or break another's assertion.
    public class CrudServiceCacheTokenGateTests
    {
        private const string ProbeId = "probe-1";

        private sealed class MintsThroughDefaultRegion;
        private sealed class ExpiresExtraKeysInTheSameRegion;
        private sealed class MintsThroughItsOwnRegion;
        private sealed class MintsThroughItsOwnRegionAndOptsBackIn;

        [Fact]
        public async Task GetAsync_ServiceMintsThroughDefaultRegion_InvalidationDuringLoad_IsNotCached()
        {
            // The default shape: the gate is open, the token is captured before the load, and the mid-load
            // invalidation cancels it — so the stale value must not survive in the cache.
            var service = new ProbeCrudService<MintsThroughDefaultRegion> { ExpireDuringLoad = true };

            var first = await service.GetAsync([ProbeId]);
            var second = await service.GetAsync([ProbeId]);

            Assert.Equal(1, first.Single().Version);
            Assert.Equal(2, service.LoadCalls);
            Assert.Equal(2, second.Single().Version);
        }

        [Fact]
        public async Task GetAsync_ServiceOverridesOnlyClearCache_InvalidationDuringLoad_IsNotCached()
        {
            // Regression guard for the CustomerOrderService shape: ClearCache is overridden (without calling
            // base) but expires the same region the default ConfigureCache mints into. Conditioning the gate
            // on ClearCache too would silently drop the fix here — the one place it was measured.
            var service = new ClearCacheOnlyProbeCrudService<ExpiresExtraKeysInTheSameRegion> { ExpireDuringLoad = true };

            await service.GetAsync([ProbeId]);
            var second = await service.GetAsync([ProbeId]);

            Assert.Equal(2, service.LoadCalls);
            Assert.Equal(2, second.Single().Version);
        }

        [Fact]
        public async Task GetAsync_ServiceOverridesConfigureCache_DoesNotCreateTokensInTheDefaultRegion()
        {
            // The mint is redirected to ProbeCacheRegion, so this service gains nothing from the early
            // capture. It must not pay for it either: GenericCachingRegion<TModel> stays untouched.
            var service = new OwnRegionProbeCrudService<MintsThroughItsOwnRegion>();

            await service.GetAsync([ProbeId]);

            Assert.Empty(GetDefaultRegionTokenKeys<ProbeModel<MintsThroughItsOwnRegion>>());

            // Proves the assertion above is not vacuous — the overridden ConfigureCache did run and did
            // register the id in the region this service actually expires.
            Assert.Contains(
                ProbeCacheRegion<MintsThroughItsOwnRegion>.GenerateRegionTokenKey(ProbeId),
                GetRegionTokenKeys<ProbeCacheRegion<MintsThroughItsOwnRegion>>());
        }

        [Fact]
        public async Task GetAsync_ServiceOverridesConfigureCacheButOptsBackIn_CreatesTokensInTheDefaultRegion()
        {
            // The escape hatch: a subclass whose ConfigureCache override does call base restores the early
            // capture with a one-line CaptureCacheTokenBeforeLoad override.
            var service = new OptedInProbeCrudService<MintsThroughItsOwnRegionAndOptsBackIn>();

            await service.GetAsync([ProbeId]);

            Assert.Contains(
                GenericCachingRegion<ProbeModel<MintsThroughItsOwnRegionAndOptsBackIn>>.GenerateRegionTokenKey(ProbeId),
                GetDefaultRegionTokenKeys<ProbeModel<MintsThroughItsOwnRegionAndOptsBackIn>>());
        }

        private static IReadOnlyCollection<string> GetDefaultRegionTokenKeys<TModel>()
            where TModel : IEntity
        {
            return GetRegionTokenKeys<GenericCachingRegion<TModel>>();
        }

        // Reads CancellableCacheRegion<TRegion>'s private static _keyTokensDict. There is no public read
        // surface for it, and the whole point of the gate is that entries never appear there — so the
        // dictionary itself is what has to be asserted on.
        private static IReadOnlyCollection<string> GetRegionTokenKeys<TRegion>()
        {
            var field = typeof(CancellableCacheRegion<TRegion>)
                .GetField("_keyTokensDict", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(field);

            var tokens = (ConcurrentDictionary<string, CancellationTokenSource>)field.GetValue(null);

            return tokens.Keys.ToList();
        }

        private sealed class ProbeModel<TMarker> : Entity, ICloneable
        {
            public int Version { get; set; }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        private sealed class ProbeEntity<TMarker> : Entity, IDataEntity<ProbeEntity<TMarker>, ProbeModel<TMarker>>
        {
            public int Version { get; set; }

            // GetAsync never converts in either direction here — ProcessModels is overridden so the probe
            // needs no AbstractTypeFactory registration — and it never saves.
            public ProbeModel<TMarker> ToModel(ProbeModel<TMarker> model) => throw new NotSupportedException();

            public ProbeEntity<TMarker> FromModel(ProbeModel<TMarker> model, PrimaryKeyResolvingMap pkMap) => throw new NotSupportedException();

            public void Patch(ProbeEntity<TMarker> target) => throw new NotSupportedException();
        }

        private sealed class ProbeChangingEvent<TMarker>(IEnumerable<GenericChangedEntry<ProbeModel<TMarker>>> changedEntries)
            : GenericChangedEntryEvent<ProbeModel<TMarker>>(changedEntries);

        private sealed class ProbeChangedEvent<TMarker>(IEnumerable<GenericChangedEntry<ProbeModel<TMarker>>> changedEntries)
            : GenericChangedEntryEvent<ProbeModel<TMarker>>(changedEntries);

        private sealed class ProbeCacheRegion<TMarker> : CancellableCacheRegion<ProbeCacheRegion<TMarker>>;

        // Overrides nothing: mints and expires through GenericCachingRegion<TModel>.
        private class ProbeCrudService<TMarker> : CrudService<ProbeModel<TMarker>, ProbeEntity<TMarker>, ProbeChangingEvent<TMarker>, ProbeChangedEvent<TMarker>>
        {
            public ProbeCrudService()
                : base(() => new Mock<IRepository>().Object, MemoryCacheMockHelper.GetPlatformMemoryCache(), new Mock<IEventPublisher>().Object)
            {
            }

            public int LoadCalls { get; private set; }

            /// <summary>
            /// Models a writer that commits and invalidates the key WHILE this load is in flight.
            /// </summary>
            public bool ExpireDuringLoad { get; set; }

            protected override Task<IList<ProbeEntity<TMarker>>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
            {
                LoadCalls++;

                if (ExpireDuringLoad)
                {
                    GenericCachingRegion<ProbeModel<TMarker>>.ExpireTokenForKey(ProbeId);
                }

                IList<ProbeEntity<TMarker>> entities = ids
                    .Select(x => new ProbeEntity<TMarker> { Id = x, Version = LoadCalls })
                    .ToList();

                return Task.FromResult(entities);
            }

            protected override IList<ProbeModel<TMarker>> ProcessModels(IList<ProbeEntity<TMarker>> entities, string responseGroup)
            {
                return entities
                    .Select(x => new ProbeModel<TMarker> { Id = x.Id, Version = x.Version })
                    .ToList();
            }
        }

        // Mirrors vc-module-order CustomerOrderService: overrides ClearCache without calling base, but expires
        // the same region the default ConfigureCache mints into — it only adds a second key.
        // The body is documentary — the gate reacts to the override EXISTING, and no test invokes ClearCache.
        // Keep it anyway: emptying it would leave the probe no longer mirroring the shape it stands for.
        private sealed class ClearCacheOnlyProbeCrudService<TMarker> : ProbeCrudService<TMarker>
        {
            protected override void ClearCache(IList<ProbeModel<TMarker>> models)
            {
                GenericSearchCachingRegion<ProbeModel<TMarker>>.ExpireRegion();

                foreach (var model in models)
                {
                    GenericCachingRegion<ProbeModel<TMarker>>.ExpireTokenForKey(model.Id);
                }
            }
        }

        // Mirrors vc-module-inventory InventoryServiceImpl: redirects both the mint and the expire to its own
        // region and never calls base.
        private class OwnRegionProbeCrudService<TMarker> : ProbeCrudService<TMarker>
        {
            protected override void ConfigureCache(MemoryCacheEntryOptions cacheOptions, string id, ProbeModel<TMarker> model)
            {
                cacheOptions.AddExpirationToken(ProbeCacheRegion<TMarker>.CreateChangeTokenForKey(id));
            }

            protected override void ClearCache(IList<ProbeModel<TMarker>> models)
            {
                ProbeCacheRegion<TMarker>.ExpireRegion();
            }
        }

        private sealed class OptedInProbeCrudService<TMarker> : OwnRegionProbeCrudService<TMarker>
        {
            protected override bool CaptureCacheTokenBeforeLoad => true;
        }
    }
}
