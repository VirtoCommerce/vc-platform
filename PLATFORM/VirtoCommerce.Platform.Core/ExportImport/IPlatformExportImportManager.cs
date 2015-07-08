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
		void Export(Stream outStream, ModuleDescriptor[] modules, SemanticVersion platformVersion, Action<ExportImportProgressInfo> progressCallback);
		void Import(Stream inputStream, ModuleDescriptor[] modules, SemanticVersion platformVersion, Func<ExportImportProgressInfo> progressCallback);
	}
}
