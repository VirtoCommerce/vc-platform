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
		public bool HandleSettings { get; set; }
		public bool HandleSecurity { get; set; }
		/// <summary>
		/// Flag means the use of  binary data in export or import operations
		/// </summary>
		public bool HandleBinaryData { get; set; }
		public ModuleDescriptor[] Modules { get; set; }
	}
}
