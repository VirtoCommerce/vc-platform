using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.Platform.Data.Notification
{
	public class LiquidNotificationTemplateResolver : INotificationTemplateResolver
	{
		public void ResolveTemplate(Core.Notification.Notification notification)
		{
			Template templateSubject = Template.Parse(notification.NotificationTemplate.Subject);
			notification.Subject = templateSubject.Render(Hash.FromAnonymousObject(new
			{
				context = notification
			}));

			Template templateBody = Template.Parse(notification.NotificationTemplate.Body);
			notification.Body = templateBody.Render(Hash.FromAnonymousObject(new
			{
				context = notification
			}));
		}
	}
}
