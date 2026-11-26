using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
    /// <summary>
    /// Applies <see cref="PushNotificationSearchCriteria"/> to the notifications of one user, for any <see cref="IPushNotificationStorage"/>.
    /// </summary>
    public static class PushNotificationSearch
    {
        public static PushNotificationSearchResult Search(IEnumerable<PushNotification> notifications, PushNotificationSearchCriteria criteria)
        {
            var query = notifications.OrderByDescending(x => x.Created).AsQueryable();
            if (criteria.Ids != null && criteria.Ids.Length > 0)
            {
                query = query.Where(x => criteria.Ids.Contains(x.Id));
            }
            if (criteria.OnlyNew)
            {
                query = query.Where(x => x.IsNew);
            }
            if (criteria.StartDate != null)
            {
                query = query.Where(x => x.Created >= criteria.StartDate);
            }
            if (criteria.EndDate != null)
            {
                query = query.Where(x => x.Created <= criteria.EndDate);
            }

            var sortInfos = criteria.SortInfos;
            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<PushNotification>(x => x.Created), SortDirection = SortDirection.Descending } };
            }

            return new PushNotificationSearchResult
            {
                TotalCount = query.Count(),
                NewCount = query.Count(x => x.IsNew),
                NotifyEvents = query.OrderBySortInfos(sortInfos).Skip(criteria.Skip).Take(criteria.Take).ToList()
            };
        }
    }
}
