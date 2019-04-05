using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class AspsmsSmsNotificationSendingGateway : ISmsNotificationSendingGateway
    {
        private readonly AspsmsSmsGatewayOptions _options;

        public AspsmsSmsNotificationSendingGateway(AspsmsSmsGatewayOptions options)
        {
            _options = options;
        }

        public SendNotificationResult SendNotification(Notification notification)
        {
            return Task.Run(() => SendNotificationAsync(notification)).GetAwaiter().GetResult();
        }

        public bool ValidateNotification(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(notification.Recipient) && !string.IsNullOrWhiteSpace(notification.Body) && notification.Body.Length < 500;
        }

        protected async Task<SendNotificationResult> SendNotificationAsync(Notification notification)
        {
            ValidateParameters(notification);

            var result = new SendNotificationResult();

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(_options.JsonApiUri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
                {

                    var json = JsonConvert.SerializeObject(new
                    {
                        UserName = _options.AccountId,
                        Password = _options.AccountPassword,
                        Originator = _options.Sender,
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
