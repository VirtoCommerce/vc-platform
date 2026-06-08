using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.ExportImport.PushNotifications;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    /// <summary>
    /// HTTP response envelope for <c>POST /api/platform/export</c>.
    /// Carries the in-flight push notification plus — for encrypted backups — the one-time
    /// password. The password is intentionally a top-level field (not on the push
    /// notification) so it never enters push-notification storage.
    /// </summary>
    public class PlatformExportStartedResult
    {
        [JsonProperty("notification")]
        public PlatformExportPushNotification Notification { get; set; }

        /// <summary>
        /// The plaintext backup password. Present only on encrypted exports and emitted
        /// exactly once — in the HTTP response that starts the job. Subsequent push
        /// notifications carrying job progress do NOT include this field.
        /// </summary>
        [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }
}
