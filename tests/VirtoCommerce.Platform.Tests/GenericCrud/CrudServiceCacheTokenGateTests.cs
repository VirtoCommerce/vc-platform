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
    // Pins the ConfigureCache gate in CrudService.GetAsync (VCST-5627).
    //
    // Every probe type is generic over a marker: GenericCachingRegion<TModel> keys static token state on
    // TModel, so a shared model type would let one test's tokens satisfy or break another's assertion.
    public class CrudServiceCacheTokenGateTests
    {
        private const string ProbeId = "probe-1";

        private sealed class DefaultCacheToken;
        private sealed class ExtraKeysInTheSameRegion;
        private sealed class OwnCacheToken;
        private sealed class OwnCacheTokenCallingBase;

        [Fact]
        public async Task GetAsync_ServiceKeepsDefaultCacheToken_InvalidationDuringLoad_IsNotCached()
        {
            var service = new ProbeCrudService<DefaultCacheToken> { ExpireDuringLoad = true };

            var first = await service.GetAsync([ProbeId]);
            var second = await service.GetAsync([ProbeId]);

            Assert.Equal(1, first.Single().Version);
            Assert.Equal(2, service.LoadCalls);
            Assert.Equal(2, second.Single().Version);
        }

        [Fact]
        public async Task GetAsync_ServiceOverridesOnlyClearCache_InvalidationDuringLoad_IsNotCached()
        {
            // Regression guard: conditioning the gate on ClearCache too would silently drop the fix here.
            var service = new ClearCacheOnlyProbeCrudService<ExtraKeysInTheSameRegion> { ExpireDuringLoad = true };

            await service.GetAsync([ProbeId]);
            var second = await service.GetAsync([ProbeId]);

            Assert.Equal(2, service.LoadCalls);
            Assert.Equal(2, second.Single().Version);
        }

        [Fact]
        public async Task GetAsync_ServiceOverridesConfigureCache_DoesNotCreateTokensInTheDefaultRegion()
        {
            var service = new OwnRegionProbeCrudService<OwnCacheToken>();

            await service.GetAsync([ProbeId]);

            Assert.Empty(GetDefaultRegionTokenKeys<ProbeModel<OwnCacheToken>>());

            // Proves the assertion above is not vacuous: the overridden ConfigureCache did run.
            Assert.Contains(
                ProbeCacheRegion<OwnCacheToken>.GenerateRegionTokenKey(ProbeId),
                GetRegionTokenKeys<ProbeCacheRegion<OwnCacheToken>>());
        }

        [Fact]
        public async Task GetAsync_ServiceOverridesConfigureCacheButOptsBackIn_CreatesTokensInTheDefaultRegion()
        {
            var service = new OptedInProbeCrudService<OwnCacheTokenCallingBase>();

            await service.GetAsync([ProbeId]);

            Assert.Contains(
                GenericCachingRegion<ProbeModel<OwnCacheTokenCallingBase>>.GenerateRegionTokenKey(ProbeId),
                GetDefaultRegionTokenKeys<ProbeModel<OwnCacheTokenCallingBase>>());
        }

        private static List<string> GetDefaultRegionTokenKeys<TModel>()
            where TModel : IEntity
        {
            return GetRegionTokenKeys<GenericCachingRegion<TModel>>();
        }

        // _keyTokensDict has no public read surface, and "no entry ever appears" is the assertion.
        private static List<string> GetRegionTokenKeys<TRegion>()
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

            // ProcessModels is overridden, so no conversion and no AbstractTypeFactory registration.
            public ProbeModel<TMarker> ToModel(ProbeModel<TMarker> model) => throw new NotSupportedException();

            public ProbeEntity<TMarker> FromModel(ProbeModel<TMarker> model, PrimaryKeyResolvingMap pkMap) => throw new NotSupportedException();

            public void Patch(ProbeEntity<TMarker> target) => throw new NotSupportedException();
        }

        private sealed class ProbeChangingEvent<TMarker>(IEnumerable<GenericChangedEntry<ProbeModel<TMarker>>> changedEntries)
            : GenericChangedEntryEvent<ProbeModel<TMarker>>(changedEntries);

        private sealed class ProbeChangedEvent<TMarker>(IEnumerable<GenericChangedEntry<ProbeModel<TMarker>>> changedEntries)
            : GenericChangedEntryEvent<ProbeModel<TMarker>>(changedEntries);

        private sealed class ProbeCacheRegion<TMarker> : CancellableCacheRegion<ProbeCacheRegion<TMarker>>;

        private class ProbeCrudService<TMarker> : CrudService<ProbeModel<TMarker>, ProbeEntity<TMarker>, ProbeChangingEvent<TMarker>, ProbeChangedEvent<TMarker>>
        {
            public ProbeCrudService()
                : base(() => new Mock<IRepository>().Object, MemoryCacheMockHelper.GetPlatformMemoryCache(), new Mock<IEventPublisher>().Object)
            {
            }

            public int LoadCalls { get; private set; }

            /// <summary>Models a writer invalidating the key while this load is in flight.</summary>
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

        // Mirrors vc-module-order CustomerOrderService: overrides ClearCache without base, yet expires the
        // same region the default ConfigureCache uses. The body is never invoked - the gate only reacts
        // to the override existing - but emptying it would stop the probe mirroring that shape.
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

        // Mirrors vc-module-inventory InventoryServiceImpl: adds its own token and expires its own region.
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
            protected override bool KeepsDefaultCacheToken => true;
        }
    }
}
