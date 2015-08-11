using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Notifications
{
	public class Notification
	{
		public string Id { get; set; }

		public string DisplayName { get; set; }

		public string Description { get; set; }

		public bool IsEmail { get; set; }

		public bool IsSms { get; set; }

		public string Type { get; set; }

		public bool IsActive { get; set; }

		public bool IsSuccessSend { get; set; }

		public string ObjectId { get; set; }

		public string ObjectTypeId { get; set; }

		public string Language { get; set; }

		/// <summary>
		/// Type of notificaiton sending gateway
		/// </summary>
		public string SendingGateway { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public string Sender { get; set; }

		public string Recipient { get; set; }

		/// <summary>
		/// Sending attempts count
		/// </summary>
		public int AttemptCount { get; set; }

		/// <summary>
		/// Max sending attempt count, if MaxAttemptCount less or equal AttemptCount IsActive = false and IsSent = false, notification stop sending
		/// </summary>
		public int MaxAttemptCount { get; set; }

		/// <summary>
		/// Last fail sending attempt error message
		/// </summary>
		public string LastFailAttemptMessage { get; set; }

		/// <summary>
		/// Last fail sending attempt date
		/// </summary>
		public DateTime? LastFailAttemptDate { get; set; }

		/// <summary>
		/// Start sending date, if not null notification will be sending after that date
		/// </summary>
		public DateTime? StartSendingDate { get; set; }

		public DateTime? SentDate { get; set; }
	}
}