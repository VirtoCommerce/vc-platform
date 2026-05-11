using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.ExportImport.PushNotifications;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public static class ExportImportExtension
    {
        public static void Patch(this PlatformExportImportPushNotification target, ExportImportProgressInfo source)
        {
            target.Description = source.Description;
            target.ProcessedCount = source.ProcessedCount;
            // Don't let a smaller, late-arriving total clobber an upfront total set by the caller.
            if (source.TotalCount > target.TotalCount)
            {
                target.TotalCount = source.TotalCount;
            }

            if (source.Errors != null)
            {
                target.Errors ??= new List<string>();
                foreach (var error in source.Errors.Where(x => !target.Errors.Contains(x)))
                {
                    target.Errors.Add(error);
                }
            }

            if (source.ProgressLog != null && source.ProgressLog.Count > 0)
            {
                target.ProgressLog ??= new List<ProgressMessage>();
                foreach (var entry in source.ProgressLog)
                {
                    target.ProgressLog.Add(entry);
                }
            }
        }

        public static PlatformExportManifest ToManifest(this PlatformImportExportRequest request)
        {
            var retVal = request.ExportManifest;

            retVal.HandleBinaryData = request.HandleBinaryData;
            retVal.HandleSecurity = request.HandleSecurity;
            retVal.HandleSettings = request.HandleSettings;
            retVal.HandleDynamicProperties = request.HandleDynamicProperties;

            //Leave only selected modules
            retVal.Modules = retVal.Modules.Where(x => request.Modules != null && request.Modules.Contains(x.Id)).ToArray();
            return retVal;
        }
    }
}
