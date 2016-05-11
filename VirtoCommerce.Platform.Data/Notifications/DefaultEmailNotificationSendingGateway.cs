using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using SendGrid;
using System.Web;
using Exceptions;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Notifications
{
	public class DefaultEmailNotificationSendingGateway : IEmailNotificationSendingGateway
	{
		private readonly ISettingsManager _settingsManager;

		private const string sendGridUserNameSettingName = "VirtoCommerce.Platform.Notifications.SendGrid.UserName";
		private const string sendGridPasswordSettingName = "VirtoCommerce.Platform.Notifications.SendGrid.Secret";

		public DefaultEmailNotificationSendingGateway(ISettingsManager settingsManager)
		{
			if (settingsManager == null)
				throw new ArgumentNullException("settingsManager");

			_settingsManager = settingsManager;
		}

		public SendNotificationResult SendNotification(Core.Notifications.Notification notification)
		{
			var retVal = new SendNotificationResult();

			SendGridMessage mail = new SendGridMessage();
			mail.From = new MailAddress(notification.Sender);
			mail.AddTo(notification.Recipient);
			mail.Subject = notification.Subject;
			mail.Html = notification.Body;

			var userName = _settingsManager.GetSettingByName(sendGridUserNameSettingName).Value;
			var password = _settingsManager.GetSettingByName(sendGridPasswordSettingName).Value;

			var credentials = new NetworkCredential(userName, password);
			var transportWeb = new Web(credentials);
			try
			{
                Task.Run(async () => await transportWeb.DeliverAsync(mail)).Wait();
				retVal.IsSuccess = true;
			}
			catch (Exception ex)
			{
				retVal.ErrorMessage = ex.Message;

				if (ex.InnerException is InvalidApiRequestException)
				{
					var apiEx = ex.InnerException as InvalidApiRequestException;
					if (apiEx.Errors != null && apiEx.Errors.Length > 0)
					{
						retVal.ErrorMessage = string.Join(" ", apiEx.Errors);
					}
				}
			}

			return retVal;
		}

		public bool ValidateNotification(Core.Notifications.Notification notification)
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
