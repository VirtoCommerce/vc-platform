using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public abstract class EmailNotification : Notification
	{
		public EmailNotification(IEmailNotificationSendingGateway gateway) : base(gateway)
		{
		}
	}
}
