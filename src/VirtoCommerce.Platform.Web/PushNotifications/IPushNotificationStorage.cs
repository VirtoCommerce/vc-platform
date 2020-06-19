using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public interface IPushNotificationStorage
    {
        PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria);

        void AddOrUpdate(PushNotification notification);
    }
}
