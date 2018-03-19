using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

                    if (!string.IsNullOrEmpty(criteria.Language))
                    {
                        query = query.Where(x => x.LanguageCode == criteria.Language);
                    }

                    if (!string.IsNullOrEmpty(criteria.Group))
                    {
                        query = query.Where(x => x.Group == criteria.Group);
                    }

                    if (!criteria.Tenants.IsNullOrEmpty())
                    {
                        var tenants = criteria.Tenants.Where(x => !string.IsNullOrEmpty(x.TenantId) && !string.IsNullOrEmpty(x.TenantType)).ToArray();
                        if (tenants.Any())
                        {
                            Expression<Func<AssetEntryEntity, bool>> tenantsExp = null;

                            // x => x.TenantId == tenant.TenantId && x.TenantType == tenant.TenantType
                            var paramX = Expression.Parameter(typeof(AssetEntryEntity), "x");
                            var tenantId = Expression.MakeMemberAccess(paramX, typeof(AssetEntryEntity).GetMember("TenantId").First());
                            var tenantType = Expression.MakeMemberAccess(paramX, typeof(AssetEntryEntity).GetMember("TenantType").First());
                            foreach (var tenant in tenants)
                            {
                                var eqTenantId = Expression.Equal(tenantId, Expression.Constant(tenant.TenantId, typeof(string)));
                                var lambdaId = Expression.Lambda<Func<AssetEntryEntity, bool>>(eqTenantId, paramX);
                                
                                var eqTenantType = Expression.Equal(tenantType, Expression.Constant(tenant.TenantType, typeof(string)));
                                var lambdaType = Expression.Lambda<Func<AssetEntryEntity, bool>>(eqTenantType, paramX);

                                var body = Expression.AndAlso(lambdaId.Body, lambdaType.Body);
                                var tenantExp = Expression.Lambda<Func<AssetEntryEntity, bool>>(body, paramX);
                                
                                if (tenantsExp == null)
                                    tenantsExp = tenantExp;
                                else
                                {
                                    body = Expression.OrElse(tenantsExp.Body, tenantExp.Body);
                                    tenantsExp = Expression.Lambda<Func<AssetEntryEntity, bool>>(body, paramX);
                                }
                            }

                            query = query.Where(tenantsExp);
                        }
                    }
                    
                    var result = new AssetEntrySearchResult
                    {
                        TotalCount = query.Count()
                    };

                    result.Assets = query
                        .OrderBy(x => x.Name)
                        .Skip(criteria.Skip)
                        .Take(criteria.Take)
                        .ToArray()
                        .Select(x => x.ToModel(AbstractTypeFactory<AssetEntry>.TryCreateInstance(), _blobUrlResolver))
                        .ToList();
                    return result;
                }
            });
        }

        public IEnumerable<AssetEntry> GetByIds(IEnumerable<string> ids)
        {
            using (var repository = _platformRepository())
            {
                var entities = repository.AssetEntries.Where(x => ids.Contains(x.Id)).ToArray();
                return entities.Select(x => x.ToModel(AbstractTypeFactory<AssetEntry>.TryCreateInstance(), _blobUrlResolver));
            }
        }

        public void SaveChanges(IEnumerable<AssetEntry> items)
        {
            using (var repository = _platformRepository())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var dbExistingIds = items.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray();
                var dbExistEntities = repository.AssetEntries.Where(x => dbExistingIds.Contains(x.Id));
                foreach (var item in items)
                {
                    var originalEntity = dbExistEntities.FirstOrDefault(x => x.Id == item.Id);
                    var modifiedEntity = AbstractTypeFactory<AssetEntryEntity>.TryCreateInstance().FromModel(item);
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
