using System;
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
                using (var mailMessage = new MailMessage())
                {
                    var emailNotification = notification as EmailNotification;
                    //To email
                    var recipients = emailNotification.Recipient.Split(';', ',');
                    foreach (var email in recipients)
                    {
                        mailMessage.To.Add(new MailAddress(email));
                    }

                    //From email
                    mailMessage.From = new MailAddress(emailNotification.Sender);
                    mailMessage.ReplyToList.Add(mailMessage.From);

                    mailMessage.Subject = emailNotification.Subject;
                    mailMessage.Body = emailNotification.Body;
                    mailMessage.IsBodyHtml = true;
                    if (!emailNotification.CC.IsNullOrEmpty())
                    {
                        foreach (var ccEmail in emailNotification.CC)
                        {
                            mailMessage.CC.Add(new MailAddress(ccEmail));
                        }
                    }
                    if (!emailNotification.Bcc.IsNullOrEmpty())
                    {
                        foreach (var bccEmail in emailNotification.Bcc)
                        {
                            mailMessage.Bcc.Add(new MailAddress(bccEmail));
                        }
                    }

                    var login = _settingsManager.GetSettingByName(_smtpClientLoginSettingName).Value;
                    var password = _settingsManager.GetSettingByName(_smtpClientPWDSettingName).Value;
                    var host = _settingsManager.GetSettingByName(_smtpClientHostSettingName).Value;
                    var port = _settingsManager.GetSettingByName(_smtpClientPortSettingName).Value;
                    var useSsl = _settingsManager.GetValue(_smtpClientUseSslSettingName, false);

                    using (var smtpClient = new SmtpClient(host, Convert.ToInt32(port)))
                    {
                        smtpClient.Credentials = new System.Net.NetworkCredential(login, password);
                        smtpClient.EnableSsl = useSsl;

                        smtpClient.Send(mailMessage);
                    }

                }

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
