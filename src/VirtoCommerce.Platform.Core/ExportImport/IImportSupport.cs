using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public interface IImportSupport
    {
        [Obsolete("Use the cancellation-aware overload instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        Task ImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, ICancellationToken cancellationToken)
            => throw new NotImplementedException();

        Task ImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback, CancellationToken cancellationToken)
#pragma warning disable VC0014
            => ImportAsync(inputStream, options, progressCallback, new CancellationTokenWrapper(cancellationToken));
#pragma warning restore VC0014
    }
}
