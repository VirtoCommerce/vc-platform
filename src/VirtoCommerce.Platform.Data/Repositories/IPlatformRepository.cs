using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public interface IPlatformRepository : IRepository
    {
        [Obsolete("Use IAssetsRepository.AssetEntries from VirtoCommerce.AssetsModule.Data instead")]
        IQueryable<AssetEntryEntity> AssetEntries { get; }
        IQueryable<SettingEntity> Settings { get; }

        IQueryable<DynamicPropertyEntity> DynamicProperties { get; }
        IQueryable<DynamicPropertyDictionaryItemEntity> DynamicPropertyDictionaryItems { get; }
        //IQueryable<DynamicPropertyObjectValueEntity> DynamicPropertyObjectValues { get; }
        IQueryable<OperationLogEntity> OperationLogs { get; }

        Task<DynamicPropertyDictionaryItemEntity[]> GetDynamicPropertyDictionaryItemByIdsAsync(string[] ids);
        Task<DynamicPropertyEntity[]> GetDynamicPropertiesForTypesAsync(string[] objectTypes);
        Task<DynamicPropertyEntity[]> GetDynamicPropertiesByIdsAsync(string[] ids);
        Task<DynamicPropertyEntity[]> GetObjectDynamicPropertiesAsync(string[] objectTypes);

        Task<SettingEntity[]> GetObjectSettingsByNamesAsync(string[] names, string objectType, string objectId);
        [Obsolete("Use IAssetsRepository.GetAssetsByIdsAsync from VirtoCommerce.AssetsModule.Data instead")]
        Task<AssetEntryEntity[]> GetAssetsByIdsAsync(string[] ids);

        Task<OperationLogEntity[]> GetOperationLogsByIdsAsync(string[] ids);
    }
}
