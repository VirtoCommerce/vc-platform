using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public class ExportImportProgressInfo
    {
        public ExportImportProgressInfo()
        {
            Errors = new List<String>();
        }

        public string Description { get; set; }
        public long TotalCount { get; set; }
        public long ProcessedCount { get; set; }
        public long ErrorCount { get; set; }
        public ICollection<string> Errors { get; set; }

    }
}
