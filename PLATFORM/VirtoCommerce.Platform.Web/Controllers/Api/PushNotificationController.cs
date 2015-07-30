using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.PushNotification;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/pushnotifications")]
    public class PushNotificationController : ApiController
    {
        private readonly IPushNotificationManager _pushNotifier;
        public PushNotificationController(IPushNotificationManager pushNotifier)
        {
            _pushNotifier = pushNotifier;
        }

        /// <summary>
		/// api/pushnotifications?start=0&count=10&
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(PushNotificationSearchResult))]
        [Route("")]
        public IHttpActionResult Search([FromUri]PushNotificationSearchCriteria criteria)
        {
            var retVal = _pushNotifier.SearchNotifies(User.Identity.Name, criteria);
            return Ok(retVal);
        }

        [HttpGet]
        [ResponseType(typeof(void))]
        [Route("markAllAsRead")]
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

        [HttpPost]
        [Route("")]
        public IHttpActionResult Upsert(PushNotification notify)
        {
            notify.New = true;
            notify.Created = DateTime.UtcNow;
            notify.Creator = User.Identity.Name;
            _pushNotifier.Upsert(notify);
            return Ok(notify);
        }
    }
}
