using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.ExportImport
{
	public class PlatformExportImportRequest
	{
		public string FileUrl { get; set; }
		public bool HandleSecurity { get; set; }
		public bool HandleSettings { get; set; }
		public bool HandleBinaryData { get; set; }
		public string[] Modules { get; set; }
	}
}