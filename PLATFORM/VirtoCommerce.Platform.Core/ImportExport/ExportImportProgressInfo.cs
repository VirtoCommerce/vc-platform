using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.ImportExport
{
	public class ExportImportProgressInfo
	{
		public string Status { get; set; }
		public long TotalCount { get; set; }
		public long ProcessedCount { get; set; }
		public long ErrorCount { get; set; }
		public ICollection<string> Errors { get; set; }
	}
}
