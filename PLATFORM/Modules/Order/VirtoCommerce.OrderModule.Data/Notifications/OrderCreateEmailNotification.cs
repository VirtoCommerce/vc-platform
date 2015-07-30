using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Notifications
{
	public class OrderCreateEmailNotification : EmailNotification
	{
		public OrderCreateEmailNotification(IEmailNotificationSendingGateway gateway): base(gateway)
		{
		}

		[NotificationParameter("Order number")]
		public string OrderNumber { get; set; }
	}
}
