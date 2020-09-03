using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.DynamicProperties
{
    public class DynamicPropertyService : IDynamicPropertyService, IDynamicPropertyRegistrar
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;

        public DynamicPropertyService(Func<IPlatformRepository> repositoryFactory, IPlatformMemoryCache memoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
        }

        #region IDynamicPropertyRegistrar methods
        public virtual IEnumerable<string> AllRegisteredTypeNames
        {
            get
            {
                return AbstractTypeFactory<IHasDynamicProperties>.AllTypeInfos.Select(n => n.Type.FullName);
            }
        }

        public virtual void RegisterType<T>() where T : IHasDynamicProperties
        {
            if (!AbstractTypeFactory<IHasDynamicProperties>.AllTypeInfos.Any(t => t.Type == typeof(T)))
            {
                AbstractTypeFactory<IHasDynamicProperties>.RegisterType<T>();
            }
        }

        #endregion
        #region IDynamicPropertyService Members

        public virtual async Task<DynamicProperty[]> GetDynamicPropertiesAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), "GetDynamicPropertiesAsync", string.Join("-", ids));
            return await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                //Add cache  expiration token
                cacheEntry.AddExpirationToken(DynamicPropertiesCacheRegion.CreateChangeToken());
                using (var repository = _repositoryFactory())
                {
                    //Optimize performance and CPU usage
                    repository.DisableChangesTracking();

                    var result = await repository.GetDynamicPropertiesByIdsAsync(ids);
                    return result.Select(x => x.ToModel(AbstractTypeFactory<DynamicProperty>.TryCreateInstance())).ToArray();
                }
            });
        }

        public virtual async Task<DynamicProperty[]> SaveDynamicPropertiesAsync(DynamicProperty[] properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                var dbExistProperties = (await repository.GetDynamicPropertiesForTypesAsync(properties.Select(x => x.ObjectType).Distinct().ToArray())).ToList();
                foreach (var property in properties)
                {
                    var originalEntity = dbExistProperties.FirstOrDefault(x => property.IsTransient() ? x.Name.EqualsInvariant(property.Name) && x.ObjectType.EqualsInvariant(property.ObjectType) : x.Id.EqualsInvariant(property.Id));
                    var modifiedEntity = AbstractTypeFactory<DynamicPropertyEntity>.TryCreateInstance().FromModel(property, pkMap);
                    if (originalEntity != null)
                    {
                        modifiedEntity.Patch(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                    }
                }
                repository.UnitOfWork.Commit();
                pkMap.ResolvePrimaryKeys();

                DynamicPropertiesCacheRegion.ExpireRegion();
            }
            return properties;
        }

        public virtual async Task DeleteDynamicPropertiesAsync(string[] propertyIds)
        {
            if (propertyIds == null)
            {
                throw new ArgumentNullException(nameof(propertyIds));
            }

            using (var repository = _repositoryFactory())
            {
                var properties = repository.DynamicProperties.Where(p => propertyIds.Contains(p.Id))
                                           .ToList();

                foreach (var property in properties)
                {
                    repository.Remove(property);
                }

                await repository.UnitOfWork.CommitAsync();

                DynamicPropertiesCacheRegion.ExpireRegion();
            }
        }

        #endregion

    }
}
