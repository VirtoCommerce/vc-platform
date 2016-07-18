using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Exceptions;
using SendGrid;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class SendGridEmailNotificationSendingGateway : IEmailNotificationSendingGateway
    {
        private readonly ISettingsManager _settingsManager;

        private const string _sendGridUserNameSettingName = "VirtoCommerce.Platform.Notifications.SendGrid.UserName";
        private const string _sendGridPasswordSettingName = "VirtoCommerce.Platform.Notifications.SendGrid.Secret";

        public SendGridEmailNotificationSendingGateway(ISettingsManager settingsManager)
        {
            if (settingsManager == null)
                throw new ArgumentNullException("settingsManager");

            _settingsManager = settingsManager;
        }

        public SendNotificationResult SendNotification(Notification notification)
        {
            var retVal = new SendNotificationResult();

            var mail = new SendGridMessage();
            mail.From = new MailAddress(notification.Sender);
            mail.ReplyTo = new[] { mail.From };
            mail.AddTo(notification.Recipient);
            mail.Subject = notification.Subject;
            mail.Html = notification.Body;

            var userName = _settingsManager.GetSettingByName(_sendGridUserNameSettingName).Value;
            var password = _settingsManager.GetSettingByName(_sendGridPasswordSettingName).Value;

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

                var invalidApiRequestException = ex.InnerException as InvalidApiRequestException;
                if (invalidApiRequestException != null)
                {
                    if (invalidApiRequestException.Errors != null && invalidApiRequestException.Errors.Length > 0)
                    {
                        retVal.ErrorMessage = string.Join(" ", invalidApiRequestException.Errors);
                    }
                }
            }

            return retVal;
        }

        public bool ValidateNotification(Notification notification)
        {
            var retVal = ValidateNotificationRecipient(notification.Recipient);
            return retVal;
        }

        private static bool ValidateNotificationRecipient(string recipient)
        {
            try
            {
                new MailAddress(recipient);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
