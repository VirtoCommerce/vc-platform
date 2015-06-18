using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.Platform.Data.Notification
{
	public class DefaultEmailNotificationSendingGateway : IEmailNotificationSendingGateway
	{
		public SendNotificationResult SendNotification(Core.Notification.Notification notification)
		{
			var retVal = new SendNotificationResult();

			return retVal;
		}

		public bool ValidateNotification(Core.Notification.Notification notification)
		{
			var retVal = false;
			retVal = ValidateNotificationRecipient(notification.Recipient);
			return retVal;
		}

		public bool ValidateNotificationRecipient(string recipient)
		{
			try
			{
				MailAddress mailAddress = new MailAddress(recipient);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}
