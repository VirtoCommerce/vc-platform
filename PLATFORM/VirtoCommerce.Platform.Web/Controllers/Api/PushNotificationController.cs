using System;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/pushnotifications")]
    public class PushNotificationController : ApiController
    {
        private readonly IPushNotificationManager _pushNotifier;
        public PushNotificationController(IPushNotificationManager pushNotifier)
        {
            _pushNotifier = pushNotifier;
        }

        /// <summary>
        /// Search push notifications
        /// </summary>
        /// <param name="criteria">Search parameters.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(PushNotificationSearchResult))]
        public IHttpActionResult Search([FromUri]PushNotificationSearchCriteria criteria)
        {
            var retVal = _pushNotifier.SearchNotifies(User.Identity.Name, criteria);
            return Ok(retVal);
        }

        /// <summary>
        /// Mark all notifications as read
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("markAllAsRead")]
        [ResponseType(typeof(PushNotificationSearchResult))]
        public IHttpActionResult MarkAllAsRead()
        {
            var criteria = new PushNotificationSearchCriteria { OnlyNew = true, Start = 0, Count = int.MaxValue };
            var retVal = _pushNotifier.SearchNotifies(User.Identity.Name, criteria);
            foreach (var notifyEvent in retVal.NotifyEvents)
            {
                notifyEvent.New = false;
                _pushNotifier.Upsert(notifyEvent);
            }

            return Ok(retVal);
        }
     
    }
}
