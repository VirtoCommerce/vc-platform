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
    [RoutePrefix("api/mp/notification")]
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
        [Route("")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendNotification(SendDynamicMerchandisingNotificationRequest request)
        {
            var store = _storeService.GetById(request.StoreId);

            if (store == null)
                throw new NullReferenceException(string.Format("no store with this id = {0}", request.StoreId));

            if (string.IsNullOrEmpty(store.Email) && string.IsNullOrEmpty(store.AdminEmail))
                throw new NullReferenceException(string.Format("set email or admin email for store with id = {0}", request.StoreId));

            var notification = _notificationManager.GetNewNotification<DynamicMerchandisingNotification>(request.StoreId, "Store", request.Language);

            notification.Recipient = !string.IsNullOrEmpty(store.Email) ? store.Email : store.AdminEmail;
            notification.IsActive = true;
            notification.Type = request.Type;
            notification.Fields = request.Fields;

            _notificationManager.ScheduleSendNotification(notification);

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}