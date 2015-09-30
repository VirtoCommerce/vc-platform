namespace VirtoCommerce.Platform.Core.Security
{
    public static class PredefinedPermissions
    {
        public const string ModuleQuery = "platform:module:read",
            ModuleManage = "platform:module:manage";
        public const string SettingManage = "platform:setting:manage";
        public const string DynamicPropertiesQuery = "core:fulfillment:read",
            DynamicPropertiesCreate = "core:fulfillment:create",
            DynamicPropertiesAccess = "core:fulfillment:access",
            DynamicPropertiesUpdate = "core:fulfillment:update",
            DynamicPropertiesDelete = "core:fulfillment:delete";
        public const string SecurityQuery = "platform:security:read",
            SecurityCreate = "platform:security:create",
            SecurityAccess = "platform:security:access",
            SecurityUpdate = "platform:security:update",
            SecurityDelete = "platform:security:delete";
        public const string SecurityCallApi = "security:call_api";
        public const string BackgroundJobsManage = "background_jobs:manage",
                PlatformExportImport = "platform:exportImport";

        public static Permission[] Permissions { get; private set; }

        static PredefinedPermissions()
        {
            Permissions = new[]
            {
                new Permission
                {
                    Id = ModuleQuery,
                    Name = "Browse modules",
                    Description = "Permission to open modules menu and browse data.",
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
                    Id = SettingManage,
                    Name = "Manage settings",
                    Description = "Permission to manage settings.",
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
                    Name = "Read dynamic properties data",
                    Description = "Permission to browse and read dynamic properties data.",
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
                },
                new Permission
                {
                    Id = SecurityQuery,
                    Name = "Browse security data",
                    Description = "Permission to open security menu and view roles and users data.",
                    ModuleId = "VirtoCommerce.Platform",
                    GroupName = "Security"
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
                    Name = "Read security data",
                    Description = "Permission to browse and read security data.",
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
                }
            };
        }
    }
}
