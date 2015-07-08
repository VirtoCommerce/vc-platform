using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ExportImport
{
	public interface ISupportExportModule
	{
		void DoExport(Stream outStream, Action<ExportImportProgressInfo> progressCallback);
	}
}
