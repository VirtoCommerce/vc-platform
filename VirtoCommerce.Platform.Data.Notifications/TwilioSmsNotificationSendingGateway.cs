using System;
using System.Threading.Tasks;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class TwilioSmsNotificationSendingGateway : ISmsNotificationSendingGateway
    {
        private readonly ISettingsManager _settingsManager;

        private const string _accountIdSettingName = "VirtoCommerce.Platform.Notifications.Twilio.AccountSid";
        private const string _accountAuthTokenSettingName = "VirtoCommerce.Platform.Notifications.Twilio.AuthToken";
        private const string _senderSettingName = "VirtoCommerce.Platform.Notifications.Twilio.From";

        public TwilioSmsNotificationSendingGateway(ISettingsManager settingsManager)
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
            var accountId = _settingsManager.GetSettingByName(_accountIdSettingName).Value;
            var authToken = _settingsManager.GetSettingByName(_accountAuthTokenSettingName).Value;
            var sender = _settingsManager.GetSettingByName(_senderSettingName).Value;

            TwilioClient.Init(accountId, authToken);

            try
            {
                var message = await MessageResource.CreateAsync(
                    body: notification.Body,
                    from: new Twilio.Types.PhoneNumber(sender),
                    to: notification.Recipient
                );

                result.IsSuccess = message.Status != MessageResource.StatusEnum.Failed;
                if (!result.IsSuccess)
                {
                    result.ErrorMessage = $"Twilio sending failed. Error code: {message.ErrorCode}. Error message: {message.ErrorMessage}.";
                }
            }
            catch (ApiException e)
            {
                result.ErrorMessage = $"Twilio Error {e.Code} - {e.MoreInfo}. Exception: {e.Message}";
            }
            catch (Exception e)
            {
                result.ErrorMessage = $"Twilio sending failed. Exception: {e.Message}";
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
