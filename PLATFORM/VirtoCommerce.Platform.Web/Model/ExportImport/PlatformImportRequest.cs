using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.ExportImport
{
	public class PlatformImportRequest
	{
		public string FileUrl { get; set; }
		public string[] Modules { get; set; }
	}
}