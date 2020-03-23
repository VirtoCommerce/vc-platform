using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public interface IPlatformExportImportManager
    {
        PlatformExportManifest GetNewExportManifest(string author);
        PlatformExportManifest ReadExportManifest(Stream stream);
        Task ExportAsync(Stream outStream, PlatformExportManifest exportOptions, Action<ExportImportProgressInfo> progressCallback, ICancellationToken сancellationToken);
        Task ImportAsync(Stream inputStream, PlatformExportManifest importOptions, Action<ExportImportProgressInfo> progressCallback, ICancellationToken сancellationToken);
    }
}
