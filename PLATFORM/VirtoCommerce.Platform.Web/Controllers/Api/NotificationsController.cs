using DotLiquid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Web.Converters.Notifications;
using webModels = VirtoCommerce.Platform.Web.Model.Notifications;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/notification")]
    public class NotificationsController : ApiController
    {
        private readonly INotificationTemplateService _notificationTemplateService;
        private readonly INotificationManager _notificationManager;
        private readonly INotificationTemplateResolver _eventTemplateResolver;
        public NotificationsController(
            INotificationTemplateService notificationTemplateService,
            INotificationManager notificationManager,
            INotificationTemplateResolver eventTemplateResolver)
        {
            _notificationTemplateService = notificationTemplateService;
            _notificationManager = notificationManager;
            _eventTemplateResolver = eventTemplateResolver;
        }

        /// <summary>
        /// Get all registered notification types
        /// </summary>
        /// <remarks>Get all registered notification types in platform</remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(webModels.Notification[]))]
        [Route("")]
        public IHttpActionResult GetNotifications()
        {
            var notifications = _notificationManager.GetNotifications();

            return Ok(notifications.Select(s => s.ToWebModel()).ToArray());
        }


        /// <summary>
        /// Get notification template
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objectId"></param>
        /// <param name="objectTypeId"></param>
        /// <param name="language"></param>
        /// <remarks>Get notification template by notification type, objectId, objectTypeId and language</remarks>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(webModels.NotificationTemplate))]
        [Route("template/{type}/{objectId}/{objectTypeId}/{language}")]
        public IHttpActionResult GetNotificationTemplate(string type, string objectId, string objectTypeId, string language)
        {
            NotificationTemplate retVal = new NotificationTemplate();
            var notification = _notificationManager.GetNewNotification(type, objectId, objectTypeId, language);
            if (notification != null)
            {
                retVal = notification.NotificationTemplate;
            }

            return Ok(retVal.ToWebModel());
        }

        [HttpGet]
        [ResponseType(typeof(webModels.NotificationTemplate[]))]
        [Route("template/{type}/{objectId}/{objectTypeId}")]
        public IHttpActionResult GetNotificationTemplates(string type, string objectId, string objectTypeId)
        {
            List<webModels.NotificationTemplate> retVal = new List<webModels.NotificationTemplate>();
            var templates = _notificationTemplateService.GetNotificationTemplatesByNotification(type, objectId, objectTypeId);

            if (templates.Any())
            {
                retVal = templates.Select(t => t.ToWebModel()).ToList();
            }
            return Ok(retVal.ToArray());
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("template")]
        public IHttpActionResult UpdateNotificationTemplate([FromBody] webModels.NotificationTemplate notificationTemplate)
        {
            _notificationTemplateService.Update(new NotificationTemplate[] { notificationTemplate.ToCoreModel() });

            return Ok();
        }

        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("template/{id}")]
        public IHttpActionResult DeleteNotificationTemplate(string id)
        {
            _notificationTemplateService.Delete(new string[] { id });

            return Ok();
        }


        [HttpGet]
        [ResponseType(typeof(NotificationParameter[]))]
        [Route("template/{type}/preparetestdata")]
        public IHttpActionResult PrepareTest(string type)
        {
            var notification = _notificationManager.GetNewNotification(type);
            var retVal = _eventTemplateResolver.ResolveNotificationParameters(notification);

            return Ok(retVal.Select(s => s.ToWebModel()).ToArray());
        }

        [HttpPost]
        [ResponseType(typeof(webModels.Notification))]
        [Route("template/{type}/{objectId}/{objectTypeId}/{language}/resolvenotification")]
        public IHttpActionResult ResolveNotification([FromBody]List<KeyValuePair<string, string>> parameters, string type, string objectId, string objectTypeId, string language)
        {
            var notification = _notificationManager.GetNewNotification(type, objectId, objectTypeId, language);
            foreach (var param in parameters)
            {
                var property = notification.GetType().GetProperty(param.Key);
                property.SetValue(notification, param.Value);
            }
            _eventTemplateResolver.ResolveTemplate(notification);

            return Ok(notification.ToWebModel());
        }

        [HttpPost]
        [ResponseType(typeof(string))]
        [Route("template/{type}/{objectId}/{objectTypeId}/{language}/sendnotification")]
        public IHttpActionResult SendNotification([FromBody]List<KeyValuePair<string, string>> parameters, string type, string objectId, string objectTypeId, string language)
        {
            var notification = _notificationManager.GetNewNotification(type, objectId, objectTypeId, language);
            foreach (var param in parameters)
            {
                var property = notification.GetType().GetProperty(param.Key);
                property.SetValue(notification, param.Value);
            }
            var result = _notificationManager.SendNotification(notification);

            return Ok(result.ErrorMessage);
        }

        [HttpGet]
        [ResponseType(typeof(webModels.SearchNotificationsResult))]
        [Route("journal/{objectId}/{objectTypeId}")]
        public IHttpActionResult GetNotificationJournal(string objectId, string objectTypeId, int start, int count)
        {
            var result = _notificationManager.SearchNotifications(new SearchNotificationCriteria() { ObjectId = objectId, ObjectTypeId = objectTypeId, Skip = start, Take = count });

            var retVal = new webModels.SearchNotificationsResult();
            retVal.Notifications = result.Notifications.Select(nt => nt.ToWebModel()).ToArray();
            retVal.TotalCount = result.TotalCount;

            return Ok(retVal);
        }

        [HttpGet]
        [ResponseType(typeof(webModels.Notification))]
        [Route("notification/{id}")]
        public IHttpActionResult GetNotification(string id)
        {
            var retVal = _notificationManager.GetNotificationById(id);

            return Ok(retVal.ToWebModel());
        }

        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("stopnotifications")]
        public IHttpActionResult StopSendingNotifications(string[] ids)
        {
            _notificationManager.StopSendingNotifications(ids);

            return Ok();
        }
    }
}