using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Web.Model.Packaging;

namespace VirtoCommerce.Platform.Web.Model.ExportImport
{
	public class ImportInfo
	{
		public ImportInfo()
		{
			Modules = new List<ModuleDescriptor>();
		}
		public PlatformExportManifest ExportManifest { get; set; }
		public ICollection<ModuleDescriptor> Modules { get; set; }
	}
}