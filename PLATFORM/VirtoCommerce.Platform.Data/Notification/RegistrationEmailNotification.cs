using DotLiquid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.Platform.Data.Notification
{
	[CLSCompliant(false)]
	[LiquidType("Login", "FirstName", "LastName")]
	public class RegistrationEmailNotification : EmailNotification
	{
		public RegistrationEmailNotification(Func<IEmailNotificationSendingGateway> emailNotificationSendingGateway) : base(emailNotificationSendingGateway)
		{

		}

		/// <summary>
		/// User login
		/// </summary>
		[Description("User login")]
		public string Login { get; set; }

		/// <summary>
		/// User firstname
		/// </summary>
		[Description("User firstname")]
		public string FirstName { get; set; }

		/// <summary>
		/// User lastname
		/// </summary>
		[Description("User lastname")]
		public string LastName { get; set; }
	}
}
