using System;
using System.Collections.Generic;
using System.Linq;
using CacheManager.Core;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Assets
{
    public class AssetEntryService : ServiceBase, IAssetEntryService, IAssetEntrySearchService
    {
        private readonly Func<IPlatformRepository> _platformRepository;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly ICacheManager<object> _cacheManager;

        public AssetEntryService(Func<IPlatformRepository> repositoryFactory, IBlobUrlResolver blobUrlResolver, ICacheManager<object> cacheManager)
        {
            _platformRepository = repositoryFactory;
            _blobUrlResolver = blobUrlResolver;
            _cacheManager = cacheManager;
        }

        public AssetEntrySearchResult SearchAssetEntries(AssetEntrySearchCriteria criteria)
        {
            var cacheKey = $"Search:{criteria?.GetHashCode()}";
            return _cacheManager.Get(cacheKey, nameof(AssetEntry), () =>
            {
                criteria = criteria ?? new AssetEntrySearchCriteria();

                using (var repository = _platformRepository())
                {
                    var query = repository.AssetEntries;

                    if (!string.IsNullOrEmpty(criteria.SearchPhrase))
                    {
                        query = query.Where(x => x.Name.Contains(criteria.SearchPhrase) || x.RelativeUrl.Contains(criteria.SearchPhrase));
                    }

                    if (!string.IsNullOrEmpty(criteria.LanguageCode))
                    {
                        query = query.Where(x => x.LanguageCode == criteria.LanguageCode);
                    }

                    if (!string.IsNullOrEmpty(criteria.Group))
                    {
                        query = query.Where(x => x.Group == criteria.Group);
                    }

                    if (!criteria.Tenants.IsNullOrEmpty())
                    {
                        var tenants = criteria.Tenants.Where(x => x.IsValid).ToArray();
                        if (tenants.Any())
                        {
                            var tenantsStrings = tenants.Select(x => x.ToString());
                            query = query.Where(x => tenantsStrings.Contains(x.TenantId + "_" + x.TenantType));
                        }
                    }

                    var result = new AssetEntrySearchResult
                    {
                        TotalCount = query.Count()
                    };

                    var sortInfos = criteria.SortInfos;
                    if (sortInfos.IsNullOrEmpty())
                    {
                        sortInfos = new[] { new SortInfo { SortColumn = "CreatedDate", SortDirection = SortDirection.Descending } };
                    }
                    query = query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id);

                    var ids = query
                        .Skip(criteria.Skip)
                        .Take(criteria.Take)
                        .Select(x => x.Id).ToList();

                    result.Results = repository.GetAssetsByIds(ids)
                        .Select(x => x.ToModel(AbstractTypeFactory<AssetEntry>.TryCreateInstance(), _blobUrlResolver))
                        .OrderBy(x => ids.IndexOf(x.Id))
                        .ToList();
                    return result;
                }
            });
        }

        public IEnumerable<AssetEntry> GetByIds(IEnumerable<string> ids)
        {
            using (var repository = _platformRepository())
            {
                var entities = repository.GetAssetsByIds(ids);
                return entities.Select(x => x.ToModel(AbstractTypeFactory<AssetEntry>.TryCreateInstance(), _blobUrlResolver));
            }
        }

        public void SaveChanges(IEnumerable<AssetEntry> entries)
        {
            using (var repository = _platformRepository())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var nonTransientEntryIds = entries.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray();
                var originalDataEntities = repository.AssetEntries.Where(x => nonTransientEntryIds.Contains(x.Id)).ToList();
                foreach (var entry in entries)
                {
                    var originalEntity = originalDataEntities.FirstOrDefault(x => x.Id == entry.Id);
                    var modifiedEntity = AbstractTypeFactory<AssetEntryEntity>.TryCreateInstance().FromModel(entry);
                    if (originalEntity != null)
                    {
                        changeTracker.Attach(originalEntity);
                        modifiedEntity.Patch(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                    }
                }
                CommitChanges(repository);

                //Reset cached items
                ResetCache();
            }
        }

        public void Delete(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            using (var repository = _platformRepository())
            {
                var items = repository.AssetEntries
                    .Where(p => ids.Contains(p.Id))
                    .ToList();

                foreach (var item in items)
                {
                    repository.Remove(item);
                }

                repository.UnitOfWork.Commit();
                ResetCache();
            }
        }

        protected virtual void ResetCache()
        {
            _cacheManager.ClearRegion(nameof(AssetEntry));
        }
    }
}
