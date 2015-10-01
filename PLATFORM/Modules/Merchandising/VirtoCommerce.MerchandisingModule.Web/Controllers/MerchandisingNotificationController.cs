using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.MerchandisingModule.Web.Model.Notificaitons;
using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.MerchandisingModule.Web.Controllers
{
    [RoutePrefix("api/mp/{storeId}/{language}")]
    public class MerchandisingNotificationController : ApiController
    {
        private readonly INotificationManager _notificationManager;
        private readonly IStoreService _storeService;

        public MerchandisingNotificationController(INotificationManager notificationManager, IStoreService storeService)
        {
            _storeService = storeService;
            _notificationManager = notificationManager;
        }

        [HttpPost]
        [Route("notification")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendNotification(string storeId, string language)
        {
            var store = _storeService.GetById(storeId);

            if (store == null)
                throw new NullReferenceException(string.Format("no store with this id = {0}", storeId));

            if (string.IsNullOrEmpty(store.Email) && string.IsNullOrEmpty(store.AdminEmail))
                throw new NullReferenceException(string.Format("set email or admin email for store with id = {0}", storeId));

            var notification = _notificationManager.GetNewNotification<DynamicMerchandisingNotification>(storeId, "Store", language);

            notification.FormType = HttpContext.Current.Request.Form["formtype"];
            notification.Recipient = !string.IsNullOrEmpty(store.Email) ? store.Email : store.AdminEmail;
            notification.IsActive = true;

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