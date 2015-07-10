using DotLiquid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.Notification
{
	[CLSCompliant(false)]
	public class RegistrationEmailNotification : EmailNotification
	{
		public RegistrationEmailNotification(IEmailNotificationSendingGateway emailNotificationSendingGateway) : base(emailNotificationSendingGateway)
		{

		}

		/// <summary>
		/// User login
		/// </summary>
		[NotificationParameter("User login")]
		public string Login { get; set; }

		/// <summary>
		/// User firstname
		/// </summary>
		[NotificationParameter("User firstname")]
		public string FirstName { get; set; }

		/// <summary>
		/// User lastname
		/// </summary>
		[NotificationParameter("User lastname")]
		public string LastName { get; set; }
	}
}
