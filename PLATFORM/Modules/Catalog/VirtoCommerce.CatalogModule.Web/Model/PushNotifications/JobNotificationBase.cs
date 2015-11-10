using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.CatalogModule.Web.Model.PushNotifications
{
    /// <summary>
    /// Base class for sending jobs status notifications.
    /// </summary>
	public class JobNotificationBase : PushNotification
	{
		public JobNotificationBase(string creator)
			: base(creator)
		{
			Errors = new List<string>();
		}
        /// <summary>
        /// Gets or sets the job finish date and time.
        /// </summary>
        /// <value>
        /// The finished.
        /// </value>
		[JsonProperty("finished")]
		public DateTime? Finished { get; set; }
        /// <summary>
        /// Gets or sets the total count of objects to process.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
		[JsonProperty("totalCount")]
		public long TotalCount { get; set; }
        /// <summary>
        /// Gets or sets the count of processed objects.
        /// </summary>
        /// <value>
        /// The processed count.
        /// </value>
		[JsonProperty("processedCount")]
		public long ProcessedCount { get; set; }
        /// <summary>
        /// Gets or sets the count of errors during processing.
        /// </summary>
        /// <value>
        /// The error count.
        /// </value>
		[JsonProperty("errorCount")]
		public long ErrorCount { get; set; }
        /// <summary>
        /// Gets or sets the errors that has occurred during processing.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
		[JsonProperty("errors")]
		public ICollection<string> Errors { get; set; }
	}
}