using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public class GetNotificationCriteria
	{
		public string Type { get; set; }
		public string ObjectId { get; set; }
		public string ObjectTypeId { get; set; }
		public string Language { get; set; }
	}
}
