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
		PlatformExportManifest ReadPlatformExportManifest(Stream stream);
		void Export(Stream outStream, PlatformExportImportOptions exportOptions, Action<ExportImportProgressInfo> progressCallback);
		void Import(Stream inputStream, PlatformExportImportOptions importOptions, Action<ExportImportProgressInfo> progressCallback);
	}
}
