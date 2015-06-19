using DotLiquid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Web.Converters.Notification;
using webModels = VirtoCommerce.Platform.Web.Model.Notification;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
	[RoutePrefix("api/notification")]
	public class NotificationsController : ApiController
    {
		private readonly INotificationTemplateService _notificationTemplateService;
		private readonly INotificationManager _notificationManager;
		private readonly INotificationTemplateResolver _notificationTemplateResolver;
		public NotificationsController(
			INotificationTemplateService notificationTemplateService,
			INotificationManager notificationManager,
			INotificationTemplateResolver notificationTemplateResolver)
		{
			_notificationTemplateService = notificationTemplateService;
			_notificationManager = notificationManager;
			_notificationTemplateResolver = notificationTemplateResolver;
		}

		[HttpGet]
		[ResponseType(typeof(webModels.Notification[]))]
		[Route("")]
		public IHttpActionResult GetNotifications()
		{
			var notifications = _notificationManager.GetNotifications();

			return Ok(notifications.Select(s => s.ToWebModel()).ToArray());
		}

		[HttpGet]
		[ResponseType(typeof(webModels.NotificationTemplate))]
		[Route("template/{type}/{objectId}")]
		public IHttpActionResult GetNotificationTemplate(string type, string objectId)
		{
			var retVal = _notificationTemplateService.GetByNotification(type, objectId);
			if(retVal == null)
			{
				var notification = _notificationManager.GetNewNotification(type);
				if(notification != null)
				{
					retVal = notification.NotificationTemplate;
				}
			}

			return Ok(retVal.ToWebModel());
		}

		[HttpPost]
		[Route("template")]
		public IHttpActionResult UpdateNotificationTemplate([FromBody] webModels.NotificationTemplate notificationTemplate)
		{
			_notificationTemplateService.Update(new NotificationTemplate[] { notificationTemplate.ToCoreModel() });

			return Ok();
		}


		[HttpGet]
		[ResponseType(typeof(string[]))]
		[Route("{type}/preparetestdata")]
		public IHttpActionResult PrepareTest(string type)
		{
			var retVal = new string[]{};
			var notification = _notificationManager.GetNewNotification(type);
			var attributeCollection = TypeDescriptor.GetAttributes(notification.GetType());
			var attribute = (LiquidTypeAttribute)attributeCollection[typeof(LiquidTypeAttribute)];
			if(attribute != null)
			{
				retVal = attribute.AllowedMembers;
			}
			return Ok(retVal);
		}

		[HttpPost]
		[ResponseType(typeof(webModels.Notification))]
		[Route("{type}/resolvenotification")]
		public IHttpActionResult ResolveNotification([FromBody]Dictionary<string, string> parameters, string type)
		{
			var notification = _notificationManager.GetNewNotification(type);
			foreach(var param in parameters)
			{
				var property = notification.GetType().GetProperty(param.Key);
				property.SetValue(notification, param.Value);
			}
			_notificationTemplateResolver.ResolveTemplate(notification);

			return Ok(notification.ToWebModel());
		}
	}
}