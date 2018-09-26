using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public interface IPushNotificationService
    {
        PushNotificationSearchResult SearchPushNotifications(string userId, PushNotificationSearchCriteria criteria);
        IEnumerable<PushNotification> GetByIds(IEnumerable<string> ids);
        void SaveChanges(IEnumerable<PushNotification> notification);
        void Delete(IEnumerable<string> ids);
    }
}
