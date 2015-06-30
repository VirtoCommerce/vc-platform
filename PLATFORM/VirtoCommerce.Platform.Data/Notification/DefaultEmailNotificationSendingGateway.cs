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
		private readonly string _sendGridUserName;
		private readonly string _sendGridPassword;

		public DefaultEmailNotificationSendingGateway(string userName, string password)
		{
			_sendGridUserName = userName;
			_sendGridPassword = password;
		}

		public SendNotificationResult SendNotification(Core.Notification.Notification notification)
		{
			var retVal = new SendNotificationResult();

			SendGridMessage mail = new SendGridMessage();
			mail.From = new MailAddress(notification.Sender);
			mail.AddTo(notification.Recipient);
			mail.Subject = notification.Subject;
			mail.Html = notification.Body;

			var credentials = new NetworkCredential(_sendGridUserName, _sendGridPassword);
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
