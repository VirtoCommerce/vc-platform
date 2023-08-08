using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Web.PushNotifications;

public class PushNotificationUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        // Return user name for compatibility with PushNotification.Creator
        return connection.User.FindFirstValue(OpenIddictConstants.Claims.Subject);
    }
}
