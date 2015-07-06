using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Notification
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

		public string SendingGateway { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public string Sender { get; set; }

		public string Recipient { get; set; }

		public int AttemptCount { get; set; }

		public int MaxAttemptCount { get; set; }

		public string LastFailAttemptMessage { get; set; }

		public DateTime? LastFailAttemptDate { get; set; }

		public DateTime? StartSendingDate { get; set; }

		public DateTime? SentDate { get; set; }
	}
}