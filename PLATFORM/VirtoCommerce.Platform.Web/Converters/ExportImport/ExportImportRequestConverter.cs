using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Web.Model.ExportImport;
using Omu.ValueInjecter;

namespace VirtoCommerce.Platform.Web.Converters.ExportImport
{
	public static class ExportImportRequestConverter
	{
		public static PlatformExportManifest ToManifest(this PlatformImportExportRequest request)
		{
			var retVal = request.ExportManifest;
			//Copy user selection to manifest
			retVal.InjectFrom(request);
			//Leave only selected modules
			retVal.Modules = retVal.Modules.Where(x => request.Modules != null && request.Modules.Contains(x.Id)).ToArray();
			return retVal;
		}

	}
}