using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    [Authorize]
    public class PushNotificationHub : Hub
    {
    }
}
