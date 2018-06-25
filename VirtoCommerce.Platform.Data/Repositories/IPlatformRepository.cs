using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public interface IPlatformRepository : IRepository
    {
        IQueryable<AssetEntryEntity> AssetEntries { get; }

        IQueryable<SettingEntity> Settings { get; }

        IQueryable<DynamicPropertyEntity> DynamicProperties { get; }
        IQueryable<DynamicPropertyDictionaryItemEntity> DynamicPropertyDictionaryItems { get; }
        IQueryable<DynamicPropertyObjectValueEntity> DynamicPropertyObjectValues { get; }

        IQueryable<AccountEntity> Accounts { get; }
        IQueryable<ApiAccountEntity> ApiAccounts { get; }
        IQueryable<RoleEntity> Roles { get; }
        IQueryable<PermissionEntity> Permissions { get; }
        IQueryable<RoleAssignmentEntity> RoleAssignments { get; }
        IQueryable<RolePermissionEntity> RolePermissions { get; }
        IQueryable<OperationLogEntity> OperationLogs { get; }

        IQueryable<NotificationEntity> Notifications { get; }
        IQueryable<NotificationTemplateEntity> NotificationTemplates { get; }

        AssetEntryEntity[] GetAssetsByIds(IEnumerable<string> ids);
        RoleEntity GetRoleById(string id);
        Task<AccountEntity> GetAccountByNameAsync(string userName, UserDetails detailsLevel);
        NotificationTemplateEntity GetNotificationTemplateByNotification(string notificationTypeId, string objectId, string objectTypeId, string language);
        DynamicPropertyDictionaryItemEntity[] GetDynamicPropertyDictionaryItems(string propertyId);
        DynamicPropertyEntity[] GetDynamicPropertiesByIds(string[] ids);
        DynamicPropertyEntity[] GetDynamicPropertiesForTypes(string[] objectTypes);
        DynamicPropertyEntity[] GetObjectDynamicProperties(string[] objectTypes, string[] objectIds);

        SettingEntity GetSettingByName(string name);
        SettingEntity[] GetAllObjectSettings(string objectType, string objectId);
    }
}
