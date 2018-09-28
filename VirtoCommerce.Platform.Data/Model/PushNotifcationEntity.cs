using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Data.Model
{
    public class PushNotificationEntity : AuditableEntity
    {
        [StringLength(128)]
        public string Type { get; set; }
        public bool IsNew { get; set; }
        public string Title { get; set; }

        public string AssemblyQualifiedType { get; set; }
        public string SourceNotificationAsJson { get; set; }

        public virtual PushNotificationEntity FromModel(PushNotification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            Id = notification.Id;
            CreatedDate = notification.Created;
            CreatedBy = notification.Creator;
            IsNew = notification.IsNew;
            Title = notification.Title;
            Type = notification.NotifyType;

            // Note: we are not using Type.AssemblyQualifiedType here, because it includes assembly version,
            // which may lead to assembly mismatch failures when updating modules or the platform itself.
            var notificationType = notification.GetType();
            AssemblyQualifiedType = $"{notificationType.FullName}, {notificationType.Assembly.GetName().Name}";

            SourceNotificationAsJson = JsonConvert.SerializeObject(notification);

            return this;
        }

        public virtual PushNotification ToModel(PushNotification notification)
        {
            notification.Id = Id;
            notification.Created = CreatedDate;
            notification.Creator = CreatedBy;
            notification.IsNew = IsNew;
            notification.NotifyType = Type;
            notification.Title = Title;

            JsonConvert.PopulateObject(SourceNotificationAsJson, notification);

            return notification;
        }

        public virtual void Patch(PushNotificationEntity target)
        {
            target.IsNew = IsNew;
            target.Title = Title;
            target.Type = Type;
            target.AssemblyQualifiedType = AssemblyQualifiedType;
            target.SourceNotificationAsJson = SourceNotificationAsJson;
        }
    }
}
