using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.OrderModule.Data.Notification
{
	[CLSCompliant(false)]
	public class OrderCreateEmailNotification : EmailNotification
	{
		public OrderCreateEmailNotification(Func<IEmailNotificationSendingGateway> gateway): base(gateway)
		{
		}

		[NotificationParameter("Order number")]
		public string OrderNumber { get; set; }
	}
}
