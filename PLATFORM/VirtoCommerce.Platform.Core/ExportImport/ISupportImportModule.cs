using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ExportImport
{
	public interface ISupportImportModule
	{
		void DoImport(Stream inputStream, Action<ExportImportProgressInfo> progressCallback);
	}
}
