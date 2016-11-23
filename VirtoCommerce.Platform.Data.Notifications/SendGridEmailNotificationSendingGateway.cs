using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class SendGridEmailNotificationSendingGateway : IEmailNotificationSendingGateway
    {
        private readonly ISettingsManager _settingsManager;

        private const string _sendGridApiKeySettingName = "VirtoCommerce.Platform.Notifications.SendGrid.ApiKey";

        public SendGridEmailNotificationSendingGateway(ISettingsManager settingsManager)
        {
            if (settingsManager == null)
                throw new ArgumentNullException("settingsManager");

            _settingsManager = settingsManager;
        }

        public SendNotificationResult SendNotification(Notification notification)
        {
            var retVal = new SendNotificationResult();
            var apiKey = _settingsManager.GetSettingByName(_sendGridApiKeySettingName).Value;
            var sendGridClient = new SendGridAPIClient(apiKey);

            var from = new Email(notification.Sender);
            var to = new Email(notification.Recipient);
            var content = new Content("text/html", notification.Body);
            var mail = new Mail(from, notification.Subject, to, content);
            mail.ReplyTo = from;

            try
            {
                Task.Run(async () => await sendGridClient.client.mail.send.post(mail.Get())).Wait();
                retVal.IsSuccess = true;
            }
            catch (Exception ex)
            {
                retVal.ErrorMessage = ex.Message;             
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
