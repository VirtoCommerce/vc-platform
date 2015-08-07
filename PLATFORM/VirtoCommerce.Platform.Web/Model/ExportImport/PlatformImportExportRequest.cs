using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.Platform.Web.Model.ExportImport
{
	public class PlatformImportExportRequest
	{
		public string FileUrl { get; set; }
		public bool HandleSecurity { get; set; }
		public bool HandleSettings { get; set; }
		public bool HandleBinaryData { get; set; }
		public string[] Modules { get; set; }
		public PlatformExportManifest ExportManifest { get; set; }
	}
}