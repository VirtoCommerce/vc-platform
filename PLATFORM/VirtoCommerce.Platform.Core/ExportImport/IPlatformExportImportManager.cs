using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public interface IPlatformExportImportManager
    {
        PlatformExportManifest GetNewExportManifest(string author);
        PlatformExportManifest ReadExportManifest(Stream stream);
        void Export(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback);
        void Import(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback);
    }
}
