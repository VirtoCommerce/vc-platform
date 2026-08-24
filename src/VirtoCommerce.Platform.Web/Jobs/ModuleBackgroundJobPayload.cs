using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Model.Modularity;

namespace VirtoCommerce.Platform.Web.Jobs
{
    /// <summary>
    /// Serializable payload for a module install/update/uninstall background job.
    /// </summary>
    public class ModuleBackgroundJobPayload
    {
        public ModuleAction Action { get; set; }

        public ModuleInstallRequest[] Modules { get; set; }

        /// <summary>Id of the <c>ModulePushNotification</c> created by the controller, so progress updates the same UI notification.</summary>
        public string NotificationId { get; set; }

        public string Creator { get; set; }

        public string Title { get; set; }

        public int TotalCount { get; set; }

        /// <summary>
        /// True for the bootstrap auto-install flow, so the handler reconstructs a
        /// <c>ModuleAutoInstallPushNotification</c> (the setup wizard listens for that notify type) instead of a plain
        /// <c>ModulePushNotification</c>.
        /// </summary>
        public bool AutoInstall { get; set; }
    }
}
