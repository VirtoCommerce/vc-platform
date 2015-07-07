using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ImportExport
{
	public interface ISupportImportModule
	{
		void DoImport(Stream inputStream, string moduleVersion, Action<ExportImportProgressInfo> progressCallback);
	}
}
