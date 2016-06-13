using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notifications
{
	public class SearchNotificationCriteria
    {
        public SearchNotificationCriteria()
        {
            RepeatHoursIntervalForFail = 5;
        }
		public int Take { get; set; }
		public int Skip { get; set; }
		public string SortOrder { get; set; }
		public string ObjectId { get; set; }
		public string ObjectTypeId { get; set; }
		public bool IsActive { get; set; }
        /// <summary>
        /// time interval used to evaluate  active notifications have failure delivery
        /// </summary>
        public int RepeatHoursIntervalForFail { get; set; }
	}
}
