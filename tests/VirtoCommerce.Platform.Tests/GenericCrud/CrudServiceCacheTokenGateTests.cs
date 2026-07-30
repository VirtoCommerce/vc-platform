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
    // Pins the pairing gate in CrudService.GetAsync. Capturing the invalidation token before the load
    // (VCST-5303) is only meaningful for a service that still owns BOTH halves of the token pairing: the
    // mint side (CreateCacheToken, reached through the default ConfigureCache) and the expire side
    // (ClearCache). CreateCacheToken memoizes into CancellableCacheRegion<T>'s process-static key-token
    // dictionary, whose only removal path is an explicit expire — so handing it to the cache helper on
    // behalf of a subclass that invalidates through a region of its own leaks one permanently-live
    // CancellationTokenSource per id into a region nothing ever sweeps (vc-module-inventory
    // InventoryServiceImpl is the real-world shape).
    //
    // Every probe type is generic over a marker so each test gets its own GenericCachingRegion<TModel>
    // static state. Sharing a model type across tests — or with CrudServiceTests, which populates
    // GenericCachingRegion<TestModel> — would let one test's tokens satisfy or break another's assertion.
    public class CrudServiceCacheTokenGateTests
    {
        private const string ProbeId = "probe-1";

        private sealed class OwnsPairing;
        private sealed class OverridesPairing;
        private sealed class OverridesPairingAndOptsBackIn;

        [Fact]
        public async Task GetAsync_ServiceOwnsThePairing_InvalidationDuringLoad_IsNotCached()
        {
            // The default shape: the gate is open, so the token is captured before the load and the
            // mid-load invalidation cancels it. The stale value must not survive in the cache.
            var service = new ProbeCrudService<OwnsPairing> { ExpireDuringLoad = true };

            var first = await service.GetAsync([ProbeId]);
            var second = await service.GetAsync([ProbeId]);

            Assert.Equal(1, first.Single().Version);
            Assert.Equal(2, service.LoadCalls);
            Assert.Equal(2, second.Single().Version);
        }

        [Fact]
        public async Task GetAsync_ServiceOverridesThePairing_DoesNotCreateTokensInTheDefaultRegion()
        {
            // ConfigureCache and ClearCache are both replaced, so this service invalidates through
            // ProbeCacheRegion and gains nothing from the early capture. It must not pay for it either:
            // GenericCachingRegion<TModel> stays untouched.
            var service = new OverriddenPairingProbeCrudService<OverridesPairing>();

            await service.GetAsync([ProbeId]);

            Assert.Empty(GetDefaultRegionTokenKeys<ProbeModel<OverridesPairing>>());

            // Proves the assertion above is not vacuous — the overridden ConfigureCache did run and did
            // register the id in the region this service actually expires.
            Assert.Contains(
                ProbeCacheRegion<OverridesPairing>.GenerateRegionTokenKey(ProbeId),
                GetRegionTokenKeys<ProbeCacheRegion<OverridesPairing>>());
        }

        [Fact]
        public async Task GetAsync_ServiceOverridesThePairingButOptsBackIn_CreatesTokensInTheDefaultRegion()
        {
            // The escape hatch: a subclass that overrides the pairing but still calls base can restore the
            // early capture with a one-line CaptureCacheTokenBeforeLoad override.
            var service = new OptedInProbeCrudService<OverridesPairingAndOptsBackIn>();

            await service.GetAsync([ProbeId]);

            Assert.Contains(
                GenericCachingRegion<ProbeModel<OverridesPairingAndOptsBackIn>>.GenerateRegionTokenKey(ProbeId),
                GetDefaultRegionTokenKeys<ProbeModel<OverridesPairingAndOptsBackIn>>());
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

        // Owns both halves of the pairing: neither ConfigureCache nor ClearCache is overridden.
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

        // Mirrors vc-module-inventory InventoryServiceImpl: replaces both halves of the pairing with its own
        // region and never calls base.
        private class OverriddenPairingProbeCrudService<TMarker> : ProbeCrudService<TMarker>
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

        private sealed class OptedInProbeCrudService<TMarker> : OverriddenPairingProbeCrudService<TMarker>
        {
            protected override bool CaptureCacheTokenBeforeLoad => true;
        }
    }
}
