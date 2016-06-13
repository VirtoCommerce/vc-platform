using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Core.ExportImport
{
	public class ExportModuleInfo : ManifestDependency
    {
		public string PartUri { get; set; }
		public string Description { get; set; }
	}
}
