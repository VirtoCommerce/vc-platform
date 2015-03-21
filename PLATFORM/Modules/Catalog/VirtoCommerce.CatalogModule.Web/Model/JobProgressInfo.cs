using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class JobProgressInfo
	{
		public JobProgressInfo()
		{
			Errors = new List<string>();
		}
		public ProgressStatus Status { get; set; }
		public DateTime? Started { get; set; }
		public DateTime? Finished { get; set; }
		public long TotalCount { get; set; }
		public long ProcessedCount { get; set; }
		public long ErrorCount { get; set; }
		public ICollection<string> Errors { get; set; }
	
	}
}
