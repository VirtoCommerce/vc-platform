using System;

namespace VirtoCommerce.Platform.Core.Notifications
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class NotificationParameterAttribute : Attribute
    {
        public NotificationParameterAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
