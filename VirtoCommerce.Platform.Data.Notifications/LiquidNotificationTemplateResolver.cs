using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DotLiquid;
using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class LiquidNotificationTemplateResolver : INotificationTemplateResolver
    {
        public void ResolveTemplate(Notification notification)
        {
            var parameters = ResolveNotificationParameters(notification);
            var myDict = new Dictionary<string, object>();

            foreach (var parameter in parameters)
            {
                myDict.Add(parameter.ParameterName, notification.GetType().GetProperty(parameter.ParameterName).GetValue(notification));
            }

            var template = notification.NotificationTemplate;

            var templateSender = Template.Parse(template.Sender);
            var sender = templateSender.Render(Hash.FromDictionary(myDict));
            if (!string.IsNullOrEmpty(sender))
            {
                notification.Sender = sender;
            }

            var templateRecipient = Template.Parse(template.Recipient);
            var recipient = templateRecipient.Render(Hash.FromDictionary(myDict));
            if (!string.IsNullOrEmpty(recipient))
            {
                notification.Recipient = recipient;
            }

            var templateSubject = Template.Parse(template.Subject);
            notification.Subject = templateSubject.Render(Hash.FromDictionary(myDict));

            var templateBody = Template.Parse(template.Body);
            notification.Body = templateBody.Render(Hash.FromDictionary(myDict));
        }


        public NotificationParameter[] ResolveNotificationParameters(Notification notification)
        {
            var retVal = new List<NotificationParameter>();

            var properties = notification.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(NotificationParameterAttribute), true).Any()).ToList();

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
                        Type = GetParameterType(property)
                    });
                }
            }

            return retVal.ToArray();
        }

        private string GetLiquidCodeOfParameter(string name)
        {
            var retVal = string.Empty;

            var regex = new Regex(
                @"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])"
            );

            if (regex.Split(name).Length > 0)
            {
                retVal = "{{ " + name.ToLower() + " }}";
            }

            return retVal;
        }

        private NotificationParameterValueType GetParameterType(PropertyInfo property)
        {
            NotificationParameterValueType retVal;
            var type = property.PropertyType;
            if (property.PropertyType.IsArray)
            {
                type = property.PropertyType.GetElementType();
            }

            if (type == typeof(int))
                retVal = NotificationParameterValueType.Integer;
            else if (type == typeof(DateTime))
                retVal = NotificationParameterValueType.DateTime;
            else if (type == typeof(decimal))
                retVal = NotificationParameterValueType.Decimal;
            else if (type == typeof(bool))
                retVal = NotificationParameterValueType.Boolean;
            else if (type == typeof(string))
                retVal = NotificationParameterValueType.String;
            else
                retVal = NotificationParameterValueType.String;

            return retVal;
        }
    }
}
