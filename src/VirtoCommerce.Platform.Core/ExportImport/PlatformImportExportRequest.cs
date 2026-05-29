using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public class PlatformImportExportRequest : ValueObject
    {
        public string FileUrl { get; set; }
        public bool HandleSecurity { get; set; }
        public bool HandleSettings { get; set; }
        public bool HandleDynamicProperties { get; set; }
        public bool HandleBinaryData { get; set; }
        public string[] Modules { get; set; }
        public PlatformExportManifest ExportManifest { get; set; }

        /// <summary>
        /// Export: when true (default), the resulting ZIP is AES-256 encrypted with a
        /// server-generated one-time password. Admins with PlatformExport permission can
        /// untick this in the UI to produce a legacy unencrypted backup for env-handoff
        /// scenarios.
        /// </summary>
        public bool PasswordProtect { get; set; } = true;

        /// <summary>
        /// Import: the password supplied by the user to decrypt an encrypted backup.
        /// Echoed nowhere; held only in-transit between the SPA and the unprotect step.
        /// </summary>
        public string Password { get; set; }
    }
}
