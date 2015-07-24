using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Core.ExportImport
{
	public class PlatformExportImportOptions
	{
		public string Author { get; set; }
		public SemanticVersion PlatformVersion { get; set; }
		public bool PlatformSettings { get; set; }
		public bool PlatformSecurity { get; set; }
		public ModuleDescriptor[] Modules { get; set; }
	}
}
