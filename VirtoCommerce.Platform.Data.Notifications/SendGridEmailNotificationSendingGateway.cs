using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using VirtoCommerce.Platform.Core.Common;
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
            _settingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));
        }

        public SendNotificationResult SendNotification(Notification notification)
        {
            return Task.Run(() => SendNotificationAsync(notification)).GetAwaiter().GetResult();
        }


        public bool ValidateNotification(Notification notification)
        {
            var retVal = ValidateNotificationRecipient(notification.Recipient);
            return retVal;
        }


        private async Task<SendNotificationResult> SendNotificationAsync(Notification notification)
        {
            var emailNotification = notification as EmailNotification;
            if (emailNotification == null)
            {
                throw new ArgumentException(nameof(notification));
            }

            var retVal = new SendNotificationResult();
            var apiKey = _settingsManager.GetSettingByName(_sendGridApiKeySettingName).Value;
            var sendGridClient = new SendGridClient(apiKey);

            var from = new EmailAddress(emailNotification.Sender);
            var to = new EmailAddress(emailNotification.Recipient);
            var content = emailNotification.Body;
            var mail = MailHelper.CreateSingleEmail(from, to, emailNotification.Subject, content, content);
            if (!emailNotification.CC.IsNullOrEmpty())
            {
                foreach (var ccEmail in emailNotification.CC)
                {
                    mail.AddCc(new EmailAddress(ccEmail));
                }
            }
            if (!emailNotification.Bcc.IsNullOrEmpty())
            {
                foreach (var bccEmail in emailNotification.Bcc)
                {
                    mail.AddBcc(new EmailAddress(bccEmail));
                }
            }
            mail.SetReplyTo(from);

            try
            {
                var result = await sendGridClient.SendEmailAsync(mail);
                retVal.IsSuccess = result.StatusCode == HttpStatusCode.Accepted;
                if (!retVal.IsSuccess)
                {
                    retVal.ErrorMessage = result.StatusCode + ":" + await result.Body.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                retVal.ErrorMessage = ex.Message;
            }
            return retVal;
        }


        private static bool ValidateNotificationRecipient(string recipient)
        {
            MailAddress mailAddress = null;

            try
            {
                mailAddress = new MailAddress(recipient);
            }
            catch (FormatException)
            {
                // Recipient address is not valid
            }

            return mailAddress != null;
        }
    }
}
