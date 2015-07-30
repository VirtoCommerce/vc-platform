using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public class ExportImportProgressInfo
    {
        public ExportImportProgressInfo(string description = null)
        {
            Errors = new List<String>();
            Description = description;
        }

        public string Description { get; set; }
        public long TotalCount { get; set; }
        public long ProcessedCount { get; set; }
		public long ErrorCount
		{
			get
			{
				return Errors != null ? Errors.Count : 0;
			}
		}
        public ICollection<string> Errors { get; set; }

    }
}
