using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Data.ExportImport
{
	public class PlatformExportManifest
	{
		public string PlatformVersion { get; set; }
		public string SystemInfo { get; set; }
		public string Author { get; set; }
		public string CheckSum { get; set; }

		public ExportModuleInfo[] Modules { get; set; }
	}

	public class ExportModuleInfo
	{
		public string ModuleId { get; set; }
		public string ModuleVersion { get; set; }
		public string PartUri { get; set; }
	}
}
