using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public interface INotificationTemplateService
	{
		NotificationTemplate GetById(string notificationTemplateId);
		NotificationTemplate GetByNotification(string notificationTypeId, string objectId);
		NotificationTemplate Create(NotificationTemplate notificationTemplate);
		void Update(NotificationTemplate[] notificationTemplates);
		void Delete(string[] notificationIds);
	}
}
