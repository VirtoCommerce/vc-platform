using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Notifications
{
#pragma warning disable S101 // Types should be named in PascalCase
    public class ASPSMSSmsNotificationSendingGateway : ISmsNotificationSendingGateway
#pragma warning restore S101 // Types should be named in PascalCase
    {
        private readonly ISettingsManager _settingsManager;

        private const string _userKeySettingName = "VirtoCommerce.Platform.Notifications.ASPSMS.UserKey";
        private const string _userPasswordSettingName = "VirtoCommerce.Platform.Notifications.ASPSMS.UserPassword";
        private const string _senderSettingName = "VirtoCommerce.Platform.Notifications.ASPSMS.Sender";

        public ASPSMSSmsNotificationSendingGateway(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));
        }

        public SendNotificationResult SendNotification(Notification notification)
        {
            return Task.Run(() => SendNotificationAsync(notification)).GetAwaiter().GetResult();
        }

        public bool ValidateNotification(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(notification.Recipient) && !string.IsNullOrWhiteSpace(notification.Body) && notification.Body.Length < 500;
        }

        private async Task<SendNotificationResult> SendNotificationAsync(Notification notification)
        {
            ValidateParameters(notification);

            var result = new SendNotificationResult();
            var accountId = _settingsManager.GetSettingByName(_userKeySettingName).Value;
            var accountPassword = _settingsManager.GetSettingByName(_userPasswordSettingName).Value;
            var sender = _settingsManager.GetSettingByName(_senderSettingName).Value;
            var ASPSMSJsonApiUri = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:SmsGateway:ASPSMSJsonApiUri", "https://json.aspsms.com/SendSimpleTextSMS");

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ASPSMSJsonApiUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
                {

                    var json = JsonConvert.SerializeObject(new
                    {
                        UserName = accountId,
                        Password = accountPassword,
                        Originator = sender,
                        Recipients = new[] { notification.Recipient },
                        MessageText = notification.Body,
                        ForceGSM7bit = false,
                    });

                    await streamWriter.WriteAsync(json);
                }

                var httpResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    dynamic apiResult = JObject.Parse(await streamReader.ReadToEndAsync());
                    var status = (string)apiResult.StatusCode ?? string.Empty;
                    result.IsSuccess = status.EqualsInvariant("1");
                    if (!result.IsSuccess)
                    {
                        result.ErrorMessage = $"ASPSMS service returns an error. Status code: {status}. Status info: {apiResult.StatusInfo}";
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"Exception occured while sending sms using ASPSMS: {ex.Message}.";
            }
            return result;
        }

        private static void ValidateParameters(Notification notification)
        {
            var smsNotification = notification as SmsNotification;
            if (smsNotification == null)
            {
                throw new ArgumentException(nameof(notification));
            }
        }
    }
}
