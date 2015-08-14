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
		public PlatformExportManifest()
		{
			Created = DateTime.UtcNow;
		}
		public string Author { get; set; }
		public string SystemInfo { get; set; }
		public string PlatformVersion { get; set; }
		/// <summary>
		/// Export or import modules settings
		/// </summary>
		public bool HandleSettings { get; set; }
		/// <summary>
		/// Export or import security objects
		/// </summary>
		public bool HandleSecurity { get; set; }
		/// <summary>
		/// Flag means the use of  binary data in export or import operations
		/// </summary>
		public bool HandleBinaryData { get; set; }
		/// <summary>
		/// List of all exported or imported modules
		/// </summary>
		public ExportModuleInfo[] Modules { get; set; }

		public DateTime Created { get; set; }
		public string Checksum { get; set; }
	}

	
}
