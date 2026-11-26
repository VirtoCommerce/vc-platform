using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/pushnotifications")]
    [Authorize]
    public class PushNotificationController : Controller
    {
        private readonly IPushNotificationManager _pushNotifier;
        public PushNotificationController(IPushNotificationManager pushNotifier)
        {
            _pushNotifier = pushNotifier;
        }

        /// <summary>
        /// SearchAsync push notifications
        /// </summary>
        /// <param name="criteria">SearchAsync parameters.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<PushNotificationSearchResult>> SearchPushNotification([FromBody] PushNotificationSearchCriteria criteria)
        {
            var retVal = await _pushNotifier.SearchNotifiesAsync(User.Identity?.Name, criteria, HttpContext.RequestAborted);
            return Ok(retVal);
        }

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("markAllAsRead")]
        public async Task<ActionResult<PushNotificationSearchResult>> MarkAllAsRead()
        {
            var notifications = await _pushNotifier.MarkAllAsReadAsync(User.Identity?.Name, HttpContext.RequestAborted);

            var result = AbstractTypeFactory<PushNotificationSearchResult>.TryCreateInstance();
            result.NotifyEvents = notifications.ToList();
            result.NewCount = result.NotifyEvents.Count;
            result.TotalCount = result.NotifyEvents.Count;

            return Ok(result);
        }
    }
}
