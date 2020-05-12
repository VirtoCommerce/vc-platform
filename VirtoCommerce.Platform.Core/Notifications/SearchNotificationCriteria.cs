using System;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Notifications
{
	public class SearchNotificationCriteria : ValueObject
    {
        public SearchNotificationCriteria()
        {
            RepeatMinutesIntervalForFail = 5 * 60;
        }
        public int Take { get; set; }
		public int Skip { get; set; }
        /// <summary>
        /// Sorting expression property1:asc;property2:desc
        /// </summary>
        public string Sort { get; set; }

        public virtual SortInfo[] SortInfos => SortInfo.Parse(Sort).ToArray();
        public string ObjectId { get; set; }
		public string ObjectTypeId { get; set; }
		public bool IsActive { get; set; }
        /// <summary>
        /// time interval used to evaluate  active notifications have failure delivery
        /// </summary>
        public int RepeatMinutesIntervalForFail { get; set; }

        [Obsolete("This property is obsolete. Use RepeatMinutesIntervalForFail instead.")]
        public int RepeatHoursIntervalForFail { get; set; }
	}
}
