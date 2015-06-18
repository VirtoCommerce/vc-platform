using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
	[RoutePrefix("api/notification")]
	public class NotificationsController : ApiController
    {
		private readonly INotificationTemplateService _notificationTemplateService;
		private readonly INotificationManager _notificationManager;
		public NotificationsController(INotificationTemplateService notificationTemplateService, INotificationManager notificationManager)
		{
			_notificationTemplateService = notificationTemplateService;
			_notificationManager = notificationManager;
		}

		[HttpGet]
		[Route("")]
		public IHttpActionResult GetNotifications(string objectId)
		{
			var notifications = _notificationManager.GetNotifications();

			return Ok();
		}
	}
}