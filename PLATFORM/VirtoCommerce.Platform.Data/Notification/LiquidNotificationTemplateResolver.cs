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
		public void ResolveSubject(Core.Notification.Notification notification)
		{
			Template template = Template.Parse(notification.Subject);
			notification.Subject = template.Render(Hash.FromAnonymousObject(new
							{
								context = notification
							}));
		}

		public void ResolveBody(Core.Notification.Notification notification)
		{
			Template template = Template.Parse(notification.Body);
			notification.Body = template.Render(Hash.FromAnonymousObject(new
			{
				context = notification
			}));
		}
	}
}
