using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Localizations;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public interface IPlatformRepository : IRepository
    {
        IQueryable<SettingEntity> Settings { get; }

        IQueryable<LocalizedItemEntity> LocalizedItems { get; }

        IQueryable<DynamicPropertyEntity> DynamicProperties { get; }
        IQueryable<DynamicPropertyDictionaryItemEntity> DynamicPropertyDictionaryItems { get; }
        //IQueryable<DynamicPropertyObjectValueEntity> DynamicPropertyObjectValues { get; }
        IQueryable<OperationLogEntity> OperationLogs { get; }
        IQueryable<RawLicenseEntity> RawLicenses { get; }

        Task<DynamicPropertyDictionaryItemEntity[]> GetDynamicPropertyDictionaryItemByIdsAsync(string[] ids);
        Task<DynamicPropertyEntity[]> GetDynamicPropertiesForTypesAsync(string[] objectTypes);
        Task<DynamicPropertyEntity[]> GetDynamicPropertiesByIdsAsync(string[] ids);
        Task<DynamicPropertyEntity[]> GetObjectDynamicPropertiesAsync(string[] objectTypes);

        Task<SettingEntity[]> GetObjectSettingsByNamesAsync(string[] names, string objectType, string objectId);

        Task<OperationLogEntity[]> GetOperationLogsByIdsAsync(string[] ids);

        IEnumerable<string> GetModifiedProperties<T>(T item) where T : class;
    }
}
