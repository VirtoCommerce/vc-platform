using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.ExportImport
{
	public class PlatformExportImportRequest
	{
		public string FileUrl { get; set; }
		public bool PlatformSecurity { get; set; }
		public bool PlatformSettings { get; set; }
		public string[] Modules { get; set; }
	}
}