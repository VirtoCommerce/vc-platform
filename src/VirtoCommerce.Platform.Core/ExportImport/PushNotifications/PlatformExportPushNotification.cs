using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Core.ExportImport.PushNotifications
{
    public class PlatformExportPushNotification : PlatformExportImportPushNotification
    {
        public PlatformExportPushNotification(string creator)
            : base(creator)
        {
        }

        [JsonProperty("downloadUrl")]
        public string DownloadUrl { get; set; }

    }
}
