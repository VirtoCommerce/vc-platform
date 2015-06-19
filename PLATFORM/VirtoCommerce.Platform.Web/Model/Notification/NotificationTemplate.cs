using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Notification
{
	public class NotificationTemplate
	{
		public string Id { get; set; }
		public string Body { get; set; }
		public string Subject { get; set; }
		public string NotificationTypeId { get; set; }
		public string ObjectId { get; set; }
	}
}