using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.MerchandisingModule.Web.Model.Notificaitons;
using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/notifications")]
    public class MerchandisingNotificationController : ApiController
    {
        private readonly INotificationManager _notificationManager;

        public MerchandisingNotificationController(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        [HttpPost]
        [Route("send/{type}/{objectId}/{objectTypeId}/{language}/{recipient}/{sender}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendNotification(string type, string objectId, string objectTypeId, string language, string recipient, string sender)
        {
            var notification = _notificationManager.GetNewNotification<DynamicMerchandisingNotification>(objectId, objectTypeId, language);

            notification.FormType = type;
            notification.Recipient = recipient;
            notification.Sender = sender;

            var form = HttpContext.Current.Request.Form;
            var dict = new Dictionary<string, string>();
            foreach(var key in form.AllKeys)
            {
                dict.Add(key, form[key]);
            }

            notification.Fields = dict;

            _notificationManager.ScheduleSendNotification(notification);

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}