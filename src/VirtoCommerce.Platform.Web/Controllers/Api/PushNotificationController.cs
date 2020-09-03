using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<PushNotificationSearchResult> SearchPushNotification(PushNotificationSearchCriteria criteria)
        {
            var retVal = _pushNotifier.SearchNotifies(User.Identity.Name, criteria);
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
            var criteria = new PushNotificationSearchCriteria { OnlyNew = true, Skip = 0, Take = int.MaxValue };
            var retVal = _pushNotifier.SearchNotifies(User.Identity.Name, criteria);
            foreach (var notifyEvent in retVal.NotifyEvents)
            {
                notifyEvent.IsNew = false;
                await _pushNotifier.SendAsync(notifyEvent);
            }

            return Ok(retVal);
        }
    }
}
