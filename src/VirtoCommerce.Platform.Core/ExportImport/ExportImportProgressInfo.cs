using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public class ExportImportProgressInfo : ValueObject
    {
        public ExportImportProgressInfo(string description = null)
        {
            Errors = new List<string>();
            ProgressLog = new List<ProgressMessage>();
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

        /// <summary>
        /// New messages emitted since the last progress callback. Acts as a delta channel:
        /// callers fill it with one or more <see cref="ProgressMessage"/> entries, the listener
        /// (e.g. the push notification) appends them to its own cumulative log, and the next
        /// progress emission starts with a fresh list.
        /// </summary>
        public ICollection<ProgressMessage> ProgressLog { get; set; }
    }
}
