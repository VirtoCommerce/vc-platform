namespace VirtoCommerce.Platform.Core.Security
{
    public static class PredefinedPermissions
    {
        public const string ModuleQuery = "platform:module:query";
        public const string ModuleManage = "platform:module:manage";
        public const string SettingManage = "platform:setting:manage";
        public const string SecurityQuery = "platform:security:query";
        public const string SecurityManage = "platform:security:manage";
        public const string SecurityCallApi = "security:call_api";
        public const string BackgroundJobsManage = "background_jobs:manage";

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
                    ModuleId = "VirtoCommerce.Platform"
                },
                new Permission
                {
                    Id = ModuleManage,
                    Name = "Manage modules",
                    Description = "Permission to manage modules.",
                    ModuleId = "VirtoCommerce.Platform"
                },
                new Permission
                {
                    Id = SettingManage,
                    Name = "Manage settings",
                    Description = "Permission to manage settings.",
                    ModuleId = "VirtoCommerce.Platform"
                },
                new Permission
                {
                    Id = SecurityQuery,
                    Name = "Browse security data",
                    Description = "Permission to open security menu and view roles and users data.",
                    ModuleId = "VirtoCommerce.Platform"
                },
                new Permission
                {
                    Id = SecurityManage,
                    Name = "Manage security data",
                    Description = "Permission to open manage roles and users.",
                    ModuleId = "VirtoCommerce.Platform"
                },
                new Permission
                {
                    Id = SecurityCallApi,
                    Name = "Call API methods",
                    Description = "Allows to make calls to Web API methods.",
                    ModuleId = "VirtoCommerce.Platform"
                },
                new Permission
                {
                    Id = BackgroundJobsManage,
                    Name = "Manage background jobs",
                    Description = "Allows to view and manage background jobs.",
                    ModuleId = "VirtoCommerce.Platform"
                }
            };

        }
    }
}
