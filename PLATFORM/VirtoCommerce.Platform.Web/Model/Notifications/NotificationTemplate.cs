using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Notifications
{
	public class NotificationTemplate
	{
		public string Id { get; set; }
		public string Body { get; set; }
		public string Subject { get; set; }
		public string NotificationTypeId { get; set; }

		/// <summary>
		/// Id of object, that used this template for sending notification
		/// </summary>
		public string ObjectId { get; set; }

		/// <summary>
		/// Type id of object, that used this template for sending notification
		/// </summary>
		public string ObjectTypeId { get; set; }

		/// <summary>
		/// Locale of template
		/// </summary>
		public string Language { get; set; }

		/// <summary>
		/// Flag, that shows if this template is default dor notification type
		/// </summary>
		public bool IsDefault { get; set; }
	}
}