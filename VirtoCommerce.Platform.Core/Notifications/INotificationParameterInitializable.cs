using System;

namespace VirtoCommerce.Platform.Core.Notifications
{
    public interface INotificationParameterInitializable
    {
        void Initialize(IServiceProvider serviceProvider, NotificationParameter[] notificationParameters);
    }
}
