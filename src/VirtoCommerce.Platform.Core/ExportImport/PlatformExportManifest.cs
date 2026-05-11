using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public class PlatformExportManifest : ValueObject
    {
        public PlatformExportManifest()
        {
            Created = DateTime.UtcNow;
        }
        public string Author { get; set; }
        public string SystemInfo { get; set; }
        public string PlatformVersion { get; set; }
        /// <summary>
        /// Export or import modules settings
        /// </summary>
        public bool HandleSettings { get; set; }
        /// <summary>
        /// Export or import dynamic properties
        /// </summary>
        public bool HandleDynamicProperties { get; set; }
        /// <summary>
        /// Export or import security objects
        /// </summary>
        public bool HandleSecurity { get; set; }
        /// <summary>
        /// Flag means the use of  binary data in export or import operations
        /// </summary>
        public bool HandleBinaryData { get; set; }
        /// <summary>
        /// List of all exported or imported modules
        /// </summary>
        public IList<ExportModuleInfo> Modules { get; set; }
        /// <summary>
        /// Additional options which will pass to modules
        /// </summary>
        public IList<ExportImportOptions> Options { get; set; } = new List<ExportImportOptions>();

        public DateTime Created { get; set; }
        public string Checksum { get; set; }

        /// <summary>
        /// Username of the admin who initiated the import job. Set at restore start time by the
        /// platform controller; not persisted into the backup .zip's Manifest.json (transient
        /// runtime context only). Used by `ImportUsersInternalAsync` to skip the caller's own
        /// account during user import so the running session isn't invalidated by a password /
        /// security-stamp overwrite. Null/empty means "no skip" (legacy behavior).
        /// </summary>
        [JsonIgnore]
        public string CallerUserName { get; set; }
    }


}
