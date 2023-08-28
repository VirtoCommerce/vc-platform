using System.Collections.Generic;
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
        public ActionResult<PushNotificationSearchResult> SearchPushNotification([FromBody] PushNotificationSearchCriteria criteria)
        {
            var retVal = _pushNotifier.SearchNotifies(User.Identity?.Name, criteria);
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
            PushNotificationSearchResult result;
            var newNotifications = new List<PushNotification>();

            var criteria = AbstractTypeFactory<PushNotificationSearchCriteria>.TryCreateInstance();
            criteria.OnlyNew = true;

            do
            {
                result = _pushNotifier.SearchNotifies(User.Identity?.Name, criteria);
                newNotifications.AddRange(result.NotifyEvents);

                foreach (var notifyEvent in result.NotifyEvents)
                {
                    notifyEvent.IsNew = false;
                    await _pushNotifier.SendAsync(notifyEvent);
                }
            }
            while (result.TotalCount > 0);

            result.NotifyEvents = newNotifications;
            result.NewCount = newNotifications.Count;
            result.TotalCount = newNotifications.Count;

            return Ok(result);
        }
    }
}
