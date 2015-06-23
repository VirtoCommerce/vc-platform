using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;
using SendGrid;
using System.Web;

namespace VirtoCommerce.Platform.Data.Notification
{
	public class DefaultEmailNotificationSendingGateway : IEmailNotificationSendingGateway
	{
		public SendNotificationResult SendNotification(Core.Notification.Notification notification)
		{
			var retVal = new SendNotificationResult();

			SendGridMessage mail = new SendGridMessage();
			mail.From = new MailAddress(notification.Sender);
			mail.AddTo(notification.Recipient);
			mail.Subject = notification.Subject;
			mail.Html = notification.Body;

			var credentials = new NetworkCredential("azure_de17492e1b941d368b0e890ae805e2b2@azure.com", "j8ddaANMU5Whzcu");
			var transportWeb = new Web(credentials);
			try
			{
				Task.Run(() => transportWeb.DeliverAsync(mail));
				retVal.IsSuccess = true;
			}
			catch (Exception ex)
			{
				retVal.ErrorMessage = ex.Message;
			}

			return retVal;
		}

		public bool ValidateNotification(Core.Notification.Notification notification)
		{
			var retVal = false;
			retVal = ValidateNotificationRecipient(notification.Recipient);
			return retVal;
		}

		private bool ValidateNotificationRecipient(string recipient)
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
