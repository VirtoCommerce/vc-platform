namespace VirtoCommerce.Platform.Core.Security
{
    public static class PredefinedPermissions
    {
        public const string SecurityCallApi = "security:call_api";
        public const string BackgroundJobsManage = "background_jobs:manage";

        public static Permission[] Permissions { get; private set; }

        static PredefinedPermissions()
        {
            Permissions = new[]
            {
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
