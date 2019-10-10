using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public abstract class PushNotification : Entity
    {
        public PushNotification(string creator)
        {
            Created = DateTime.UtcNow;
            IsNew = true;
            Id = Guid.NewGuid().ToString();
            Creator = creator;
            NotifyType = GetType().Name;
        }
        public string Creator { get; set; }
        public DateTime Created { get; set; }
        public bool IsNew { get; set; }
        public string NotifyType { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int RepeatCount { get; set; }

        public bool ItHasSameContent(PushNotification other)
        {
            return other.Title == Title && other.NotifyType == NotifyType && other.Description == Description;
        }

    }
}
