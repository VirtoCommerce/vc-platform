using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
	public interface IPlatformRepository : IRepository
	{
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

        RoleEntity GetRoleById(string id);
		AccountEntity GetAccountByName(string userName, UserDetails detailsLevel);
		NotificationTemplateEntity GetNotificationTemplateByNotification(string notificationTypeId, string objectId, string objectTypeId, string language);
		DynamicPropertyDictionaryItemEntity[] GetDynamicPropertyDictionaryItems(string propertyId);
		DynamicPropertyEntity[] GetDynamicPropertiesByIds(string[] ids);
		DynamicPropertyEntity[] GetDynamicPropertiesForType(string objectType);
		DynamicPropertyEntity[] GetObjectDynamicProperties(string[] objectTypes, string[] objectIds);
	}
}
