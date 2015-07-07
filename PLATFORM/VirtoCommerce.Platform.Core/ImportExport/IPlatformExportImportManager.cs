using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Core.ImportExport
{
	public interface IPlatformExportImportManager
	{
		ModuleDescriptor[] GetSupportedExportModules();
		ModuleDescriptor[] GetSupportedImportModules();
		Stream Export(string[] moduleIds, string platformVersion, Action<ExportImportProgressInfo> progressCallback);
		void Import(Stream stream, Func<ExportImportProgressInfo> progressCallback);
	}
}
