using System;
using System.Linq;
using System.Net.Mail;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class DefaultSmtpEmailNotificationSendingGateway : IEmailNotificationSendingGateway
    {
        private readonly ISettingsManager _settingsManager;

        private const string _smtpClientHostSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.Host";
        private const string _smtpClientPortSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.Port";
        private const string _smtpClientLoginSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.Login";
        private const string _smtpClientPWDSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.Password";
        private const string _smtpClientUseSslSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.UseSsl";

        public DefaultSmtpEmailNotificationSendingGateway(ISettingsManager settingsManager)
        {
            if (settingsManager == null)
            {
                throw new ArgumentNullException("settingsManager");
            }

            _settingsManager = settingsManager;
        }

        public SendNotificationResult SendNotification(Notification notification)
        {
            var retVal = new SendNotificationResult();

            try
            {
                MailMessage mailMsg = new MailMessage();

                var emailNotification = notification as EmailNotification;
                //To email
                var recipients = emailNotification.Recipient.Split(';', ',');
                foreach (var email in recipients)
                {
                    mailMsg.To.Add(new MailAddress(email));
                }

                //From email
                mailMsg.From = new MailAddress(emailNotification.Sender);
                mailMsg.ReplyToList.Add(mailMsg.From);

                mailMsg.Subject = emailNotification.Subject;
                mailMsg.Body = emailNotification.Body;
                mailMsg.IsBodyHtml = true;
                if (!emailNotification.CC.IsNullOrEmpty())
                {
                    foreach (var ccEmail in emailNotification.CC)
                    {
                        mailMsg.CC.Add(new MailAddress(ccEmail));
                    }
                }
                if (!emailNotification.Bcc.IsNullOrEmpty())
                {
                    foreach (var bccEmail in emailNotification.Bcc)
                    {
                        mailMsg.Bcc.Add(new MailAddress(bccEmail));
                    }
                }

                var login = _settingsManager.GetSettingByName(_smtpClientLoginSettingName).Value;
                var password = _settingsManager.GetSettingByName(_smtpClientPWDSettingName).Value;
                var host = _settingsManager.GetSettingByName(_smtpClientHostSettingName).Value;
                var port = _settingsManager.GetSettingByName(_smtpClientPortSettingName).Value;
                var useSsl = _settingsManager.GetValue(_smtpClientUseSslSettingName, false);

                SmtpClient smtpClient = new SmtpClient(host, Convert.ToInt32(port));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(login, password);
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = useSsl;

                smtpClient.Send(mailMsg);
                retVal.IsSuccess = true;
            }
            catch (Exception ex)
            {
                retVal.ErrorMessage = ex.Message + ex.InnerException;
            }

            return retVal;
        }

        public bool ValidateNotification(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
