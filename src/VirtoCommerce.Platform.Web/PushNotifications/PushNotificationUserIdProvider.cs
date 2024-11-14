using Microsoft.AspNetCore.SignalR;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.PushNotifications;

public class PushNotificationUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        // Return user name for compatibility with PushNotification.Creator
        return connection.User.GetUserName();
    }
}
