using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Notification
{
	public class Notification
	{
		public string Type { get; set; }

		public bool IsActive { get; set; }

		public bool IsSuccessSend { get; set; }

		public string ObjectId { get; set; }

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