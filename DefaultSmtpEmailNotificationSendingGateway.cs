using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class DefaultSmtpEmailNotificationSendingGateway : IEmailNotificationSendingGateway
    {
        private readonly ISettingsManager _settingsManager;

        private const string smtpClientHostSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.Host";
        private const string smtpClientPortSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.Port";
        private const string smtpClientLoginSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.Login";
        private const string smtpClientPasswordSettingName = "VirtoCommerce.Platform.Notifications.SmptClient.Password";

        public DefaultSmtpEmailNotificationSendingGateway(ISettingsManager settingsManager)
        {
            if (settingsManager == null)
                throw new ArgumentNullException("settingsManager");

            _settingsManager = settingsManager;
        }

        public SendNotificationResult SendNotification(Notification notification)
        {
            var retVal = new SendNotificationResult();

            MailMessage mailMsg = new MailMessage();

            //To email
            mailMsg.To.Add(new MailAddress(notification.Recipient));
            //From email
            mailMsg.From = new MailAddress(notification.Sender);

            mailMsg.Subject = notification.Subject;
            mailMsg.Body = notification.Body;
            mailMsg.IsBodyHtml = true;

            var login = _settingsManager.GetSettingByName(smtpClientLoginSettingName).Value;
            var password = _settingsManager.GetSettingByName(smtpClientPasswordSettingName).Value;
            var host = _settingsManager.GetSettingByName(smtpClientHostSettingName).Value;
            var port = _settingsManager.GetSettingByName(smtpClientPortSettingName).Value;

            SmtpClient smtpClient = new SmtpClient(host, Convert.ToInt32(port));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(login, password);
            smtpClient.Credentials = credentials;

            try
            {
                smtpClient.Send(mailMsg);
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
            throw new NotImplementedException();
        }
    }
}
