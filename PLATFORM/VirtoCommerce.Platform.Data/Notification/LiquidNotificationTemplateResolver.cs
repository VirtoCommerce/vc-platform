using DotLiquid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


		public NotificationParameter[] ResolveNotificationParameters(Core.Notification.Notification notification)
		{
			var retVal = new List<NotificationParameter>();

			var attributeCollection = TypeDescriptor.GetAttributes(notification.GetType());
			var attribute = (LiquidTypeAttribute)attributeCollection[typeof(LiquidTypeAttribute)];
			if (attribute != null)
			{
				foreach (var property in attribute.AllowedMembers)
				{
					var attributes = notification.GetType().GetProperty(property).GetCustomAttributes(typeof(DescriptionAttribute), true);
					retVal.Add(new NotificationParameter
					{
						ParameterName = property,
						ParameterDescription = attributes.Length > 0 ? ((DescriptionAttribute)(attributes[0])).Description : string.Empty,
						ParameterCodeInView = GetLiquidCodeOfParameter(property)
					});
				}
			}

			return retVal.ToArray();
		}

		private string GetLiquidCodeOfParameter(string name)
		{
			var retVal = string.Empty;

			Regex regex = new Regex(
				@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])"
			);

			if(regex.Split(name).Length > 0)
			{
				retVal = "{{ context." + string.Join("_", regex.Split(name)).ToLower() + " }}";
			}

			return retVal;
		}
	}
}
