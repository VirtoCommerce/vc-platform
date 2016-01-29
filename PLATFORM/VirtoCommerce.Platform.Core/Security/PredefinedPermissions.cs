namespace VirtoCommerce.Platform.Core.Security
{
    public static class PredefinedPermissions
    {
        public const string AssetAccess = "platform:asset:access",
           AssetDelete = "platform:asset:delete",
           AssetUpdate = "platform:asset:update",
           AssetCreate = "platform:asset:create",
           AssetRead = "platform:asset:read";

        public const string ModuleQuery = "platform:module:read",
            ModuleAccess = "platform:module:access",
            ModuleManage = "platform:module:manage";
        public const string SettingQuery = "platform:setting:read",
            SettingAccess = "platform:setting:access",
            SettingUpdate = "platform:setting:update";
        public const string DynamicPropertiesQuery = "platform:dynamic_properties:read",
            DynamicPropertiesCreate = "platform:dynamic_properties:create",
            DynamicPropertiesAccess = "platform:dynamic_properties:access",
            DynamicPropertiesUpdate = "platform:dynamic_properties:update",
            DynamicPropertiesDelete = "platform:dynamic_properties:delete";
        public const string NotificationQuery = "platform:notification:read",
            NotificationCreate = "platform:notification:create",
            NotificationAccess = "platform:notification:access",
            NotificationUpdate = "platform:notification:update",
            NotificationDelete = "platform:notification:delete";
        public const string SecurityQuery = "platform:security:read",
            SecurityCreate = "platform:security:create",
            SecurityAccess = "platform:security:access",
            SecurityUpdate = "platform:security:update",
            SecurityDelete = "platform:security:delete";
        public const string SecurityCallApi = "security:call_api";
        public const string BackgroundJobsManage = "background_jobs:manage";
        public const string PlatformExportImportAccess = "platform:exportImport:access",
            PlatformImport = "platform:import",
            PlatformExport = "platform:export";


        public static Permission[] Permissions { get; private set; }

        static PredefinedPermissions()
        {
            Permissions = new[]
            {
                new Permission
                {
                    Id = ModuleQuery,
                    Name = "View modules data",
                    Description = "Permission to browse and view modules.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Modules"
                },
                new Permission
                {
                    Id = ModuleAccess,
                    Name = "Open modules menu",
                    Description = "Permission to open modules menu.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Modules"
                },
                new Permission
                {
                    Id = ModuleManage,
                    Name = "Manage modules",
                    Description = "Permission to manage modules.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Modules"
                },
                new Permission
                {
                    Id = SettingAccess,
                    Name = "Open settings menu",
                    Description = "Permission to open settings menu.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Settings"
                },
                new Permission
                {
                    Id = SettingQuery,
                    Name = "View settings",
                    Description = "Permission to browse and view settings data.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Settings"
                },
                new Permission
                {
                    Id = SettingUpdate,
                    Name = "Update dynamic property",
                    Description = "Permission to update settings data.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Settings"
                },
                new Permission
                {
                    Id = DynamicPropertiesAccess,
                    Name = "Open dynamic properties menu",
                    Description = "Permission to open dynamic properties menu.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Settings"
                },
                new Permission
                {
                    Id = DynamicPropertiesCreate,
                    Name = "Create a dynamic property",
                    Description = "Permission to create a dynamic property.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Settings"
                },
                new Permission
                {
                    Id = DynamicPropertiesDelete,
                    Name = "Delete dynamic property",
                    Description = "Permission to delete dynamic property.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Settings"
                },
                new Permission
                {
                    Id = DynamicPropertiesQuery,
                    Name = "View dynamic properties data",
                    Description = "Permission to browse and view dynamic properties data.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Settings"
                },
                new Permission
                {
                    Id = DynamicPropertiesUpdate,
                    Name = "Update dynamic property",
                    Description = "Permission to update dynamic properties data.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Settings"
                },new Permission
                {
                    Id = NotificationAccess,
                    Name = "Open notifications menu",
                    Description = "Permission to open notifications menu.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Notifications"
                },
                new Permission
                {
                    Id = NotificationCreate,
                    Name = "Create notification template",
                    Description = "Permission to create notification templates.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Notifications"
                },
                new Permission
                {
                    Id = NotificationDelete,
                    Name = "Delete notification template",
                    Description = "Permission to delete notification templates.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Notifications"
                },
                new Permission
                {
                    Id = NotificationQuery,
                    Name = "View notification templates",
                    Description = "Permission to browse and view notification templates.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Notifications"
                },
                new Permission
                {
                    Id = NotificationUpdate,
                    Name = "Update notification template",
                    Description = "Permission to update notification templates.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Notifications"
                },
                new Permission
                {
                    Id = SecurityAccess,
                    Name = "Open security menu",
                    Description = "Permission to open security menu.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Security"
                },
                new Permission
                {
                    Id = SecurityCreate,
                    Name = "Create accounts and roles",
                    Description = "Permission to create security accounts and roles.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Security"
                },
                new Permission
                {
                    Id = SecurityDelete,
                    Name = "Delete accounts and roles",
                    Description = "Permission to delete accounts and roles.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Security"
                },
                new Permission
                {
                    Id = SecurityQuery,
                    Name = "View security data",
                    Description = "Permission to browse and view security data.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Security"
                },
                new Permission
                {
                    Id = SecurityUpdate,
                    Name = "Update accounts and roles",
                    Description = "Permission to update accounts and roles.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Security"
                },
                new Permission
                {
                    Id = SecurityCallApi,
                    Name = "Call Web API methods",
                    Description = "Allows to make calls to Web API methods.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Web API"
                },
                new Permission
                {
                    Id = BackgroundJobsManage,
                    Name = "Manage background jobs",
                    Description = "Allows to view and manage background jobs.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Background Jobs"
                },
                new Permission
                {
                    Id = AssetAccess,
                    Name = "Access to asset management",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Asset management"
                },
                new Permission
                {
                    Id = AssetCreate,
                    Name = "Create assets",
                    Description = "Permission to create assets.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Asset management"
                },
                new Permission
                {
                    Id = AssetDelete,
                    Name = "Delete assets",
                    Description = "Permission to delete assets.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Asset management"
                },
                new Permission
                {
                    Id = AssetRead,
                    Name = "View assets",
                    Description = "Permission to browse and view assets.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Asset management"
                },
                new Permission
                {
                    Id = AssetUpdate,
                    Name = "Update assets",
                    Description = "Permission to update assets.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Asset management"
                },
                 new Permission
                {
                    Id = PlatformExportImportAccess,
                    Name = "Platform export&import access",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Platform export&import"
                },
                  new Permission
                {
                    Id = PlatformExport,
                    Name = "Platform export",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Platform export&import"
                },
                   new Permission
                {
                    Id = PlatformImport,
                    Name = "Platform import",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Platform export&import"
                }
            };
        }
    }
}
