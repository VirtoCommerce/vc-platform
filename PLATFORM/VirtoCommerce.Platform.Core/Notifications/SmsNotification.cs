using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notifications
{
	public class SmsNotification : Notification
	{
		public SmsNotification(ISmsNotificationSendingGateway gateway) : base(gateway)
		{

		}
	}
}
