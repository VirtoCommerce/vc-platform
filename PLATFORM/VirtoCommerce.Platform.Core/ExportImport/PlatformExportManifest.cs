using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Core.ExportImport
{
	public class PlatformExportManifest
	{
		public string PlatformVersion { get; set; }
		public string SystemInfo { get; set; }
		public DateTime Created { get; set; }
		public string Author { get; set; }
		public string CheckSum { get; set; }

		/// <summary>
		/// Part for platform accounts and acl
		/// </summary>
		public string SecurityPartUri { get; set; }
		/// <summary>
		/// Part for platform settings
		/// </summary>
		public string PlatformSettingsPartUri { get; set; }
		/// <summary>
		/// Modules parts information 
		/// </summary>
		public ExportModuleInfo[] Modules { get; set; }
	}

	public class ExportModuleInfo
	{
		public string ModuleId { get; set; }
		public string ModuleVersion { get; set; }
		public string PartUri { get; set; }
	}
}
