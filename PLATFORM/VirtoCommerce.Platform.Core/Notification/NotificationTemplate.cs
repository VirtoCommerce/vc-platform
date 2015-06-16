using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public class NotificationTemplate
	{
		public string Id { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string CreatedBy { get; set; }
		public string ModifiedBy { get; set; }

		/// <summary>
		/// Id of object that create notification
		/// </summary>
		public string ObjectId { get; set; }

		/// <summary>
		/// Id of notification type
		/// </summary>
		public string NotificationTypeId { get; set; }

		/// <summary>
		/// Display name of template
		/// </summary>
		public string DisplayName { get; set; }

		/// <summary>
		/// Subject of notification
		/// </summary>
		public string Subject { get; set; }

		/// <summary>
		/// Engine that resolve this template
		/// </summary>
		public string TemplateEngine { get; set; }

		/// <summary>
		/// Body of notification
		/// </summary>
		public string Body { get; set; }
	}
}
