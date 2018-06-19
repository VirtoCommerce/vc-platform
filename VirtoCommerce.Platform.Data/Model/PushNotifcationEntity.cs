using System;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Repositories.Migrations;

namespace VirtoCommerce.Platform.Data.Model
{
    public class PushNotificationEntity : AuditableEntity
    {
        public PushNotificationEntity()
        {
            Type = GetType().Name;
        }

        [StringLength(128)]
        public string Type { get; set; }
        public bool IsNew { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RepeatCount { get; set; }

        public virtual PushNotificationEntity FromModel(Core.PushNotifications.PushNotification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            Id = notification.Id;
            Description = notification.Description;
            Title = notification.Title;
            IsNew = notification.IsNew;
            RepeatCount = notification.RepeatCount;
            Type = notification.NotifyType;

            return this;
        }

        public Core.PushNotifications.PushNotification ToModel(Core.PushNotifications.PushNotification notifcation)
        {
            notifcation.Id = Id;
            notifcation.Created = CreatedDate;
            notifcation.Creator = CreatedBy;
            notifcation.Description = Description;
            notifcation.IsNew = IsNew;
            notifcation.NotifyType = Type;
            notifcation.RepeatCount = RepeatCount;
            notifcation.Title = Title;

            return notifcation;
        }

        public virtual void Patch( PushNotificationEntity target)
        {
            target.Description = Description;
            target.IsNew = IsNew;
            target.RepeatCount = RepeatCount;
            target.Title = Title;
            target.Type = Type;

        }

    }
}
