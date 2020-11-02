using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public class PlatformRepositoryInMemory : BaseRepositoryInMemory, IPlatformRepository
    {
        public PlatformRepositoryInMemory()
        {
            Set(new InMemoryAsyncEnumerable<AssetEntryEntity>(new List<AssetEntryEntity>()));
            Set(new InMemoryAsyncEnumerable<SettingEntity>(new List<SettingEntity>()));
            Set(new InMemoryAsyncEnumerable<DynamicPropertyEntity>(new List<DynamicPropertyEntity>()));
            Set(new InMemoryAsyncEnumerable<DynamicPropertyDictionaryItemEntity>(new List<DynamicPropertyDictionaryItemEntity>()));
            Set(new InMemoryAsyncEnumerable<OperationLogEntity>(new List<OperationLogEntity>()));
        }


        public IQueryable<AssetEntryEntity> AssetEntries => Get<AssetEntryEntity>().AsQueryable();

        public IQueryable<SettingEntity> Settings => Get<SettingEntity>().AsQueryable();

        public IQueryable<DynamicPropertyEntity> DynamicProperties => Get<DynamicPropertyEntity>().AsQueryable();

        public IQueryable<DynamicPropertyDictionaryItemEntity> DynamicPropertyDictionaryItems => Get<DynamicPropertyDictionaryItemEntity>().AsQueryable();

        public IQueryable<OperationLogEntity> OperationLogs => Get<OperationLogEntity>().AsQueryable();


        

        public virtual async Task<DynamicPropertyEntity[]> GetObjectDynamicPropertiesAsync(string[] objectTypes)
        {
            var properties = await DynamicProperties.Include(x => x.DisplayNames)
                                              .OrderBy(x => x.Name)
                                              .Where(x => objectTypes.Contains(x.ObjectType)).ToArrayAsync();
            return properties;
        }

        public virtual async Task<DynamicPropertyDictionaryItemEntity[]> GetDynamicPropertyDictionaryItemByIdsAsync(string[] ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return Array.Empty<DynamicPropertyDictionaryItemEntity>();
            }

            var retVal = await DynamicPropertyDictionaryItems.Include(x => x.DisplayNames)
                                     .Where(x => ids.Contains(x.Id))
                                     .ToArrayAsync();
            return retVal;
        }

        public virtual async Task<DynamicPropertyEntity[]> GetDynamicPropertiesByIdsAsync(string[] ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return Array.Empty<DynamicPropertyEntity>();
            }

            var retVal = await DynamicProperties.Include(x => x.DisplayNames)
                                          .Where(x => ids.Contains(x.Id))
                                          .OrderBy(x => x.Name)
                                          .ToArrayAsync();
            return retVal;
        }

        public virtual async Task<DynamicPropertyEntity[]> GetDynamicPropertiesForTypesAsync(string[] objectTypes)
        {
            if (objectTypes.IsNullOrEmpty())
            {
                return Array.Empty<DynamicPropertyEntity>();
            }

            var retVal = await DynamicProperties.Include(p => p.DisplayNames)
                                          .Where(p => objectTypes.Contains(p.ObjectType))
                                          .OrderBy(p => p.Name)
                                          .ToArrayAsync();
            return retVal;
        }


        public virtual async Task<SettingEntity[]> GetObjectSettingsByNamesAsync(string[] names, string objectType, string objectId)
        {
            var result = await Settings.Include(x => x.SettingValues)
                                 .Where(x => x.ObjectId == objectId && x.ObjectType == objectType)
                                 .Where(x => names.Contains(x.Name))
                                 .OrderBy(x => x.Name)
                                 .ToArrayAsync();
            return result;
        }

        public async Task<AssetEntryEntity[]> GetAssetsByIdsAsync(string[] ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return Array.Empty<AssetEntryEntity>();
            }

            return await AssetEntries.Where(x => ids.Contains(x.Id)).ToArrayAsync();
        }

        public async Task<OperationLogEntity[]> GetOperationLogsByIdsAsync(string[] ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return Array.Empty<OperationLogEntity>();
            }

            return await OperationLogs.Where(x => ids.Contains(x.Id)).ToArrayAsync();
        }
    }


    
}
