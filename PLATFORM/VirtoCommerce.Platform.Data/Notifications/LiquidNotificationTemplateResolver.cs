using DotLiquid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.Platform.Data.Notifications
{
	public class LiquidNotificationTemplateResolver : INotificationTemplateResolver
	{
		public void ResolveTemplate(Core.Notifications.Notification notification)
		{
			var parameters = ResolveNotificationParameters(notification);
			Dictionary<string, object> myDict = new Dictionary<string, object>();
			foreach(var parameter in parameters)
			{
				myDict.Add(parameter.ParameterName, notification.GetType().GetProperty(parameter.ParameterName).GetValue(notification));
			}

			Template templateSubject = Template.Parse(notification.NotificationTemplate.Subject);
			notification.Subject = templateSubject.Render(Hash.FromDictionary(myDict));

			Template templateBody = Template.Parse(notification.NotificationTemplate.Body);
			notification.Body = templateBody.Render(Hash.FromDictionary(myDict));
		}


		public NotificationParameter[] ResolveNotificationParameters(Core.Notifications.Notification notification)
		{
			var retVal = new List<NotificationParameter>();

			List<PropertyInfo> properties = notification.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(NotificationParameterAttribute), true).Any()).ToList();

			if (properties.Count > 0)
			{
				foreach (var property in properties)
				{
					var attributes = property.GetCustomAttributes(typeof(NotificationParameterAttribute), true);
					retVal.Add(new NotificationParameter
					{
						ParameterName = property.Name,
						ParameterDescription = attributes.Length > 0 ? ((NotificationParameterAttribute)(attributes[0])).Description : string.Empty,
						ParameterCodeInView = GetLiquidCodeOfParameter(property.Name),
                        IsDictionary = property.PropertyType.IsAssignableFrom(typeof(IDictionary)),
                        IsArray = property.PropertyType.IsArray,
                        Type = GetParameterType(property.PropertyType.Name)
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
				retVal = "{{ " + name.ToLower() + " }}";
			}

			return retVal;
		}

        private NotificationParameterValueType GetParameterType(string name)
        {
            NotificationParameterValueType retVal;

            switch(name)
            {
                case "Integer":
                    retVal = NotificationParameterValueType.Integer;
                    break;

                case "Decimal":
                    retVal = NotificationParameterValueType.Decimal;
                    break;

                case "DateTime":
                    retVal = NotificationParameterValueType.DateTime;
                    break;

                case "Boolean":
                    retVal = NotificationParameterValueType.Boolean;
                    break;

                default:
                    retVal = NotificationParameterValueType.String;
                    break;
            }

            return retVal;
        }
    }
}
