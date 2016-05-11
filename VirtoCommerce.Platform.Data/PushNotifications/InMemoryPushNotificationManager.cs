using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using core = VirtoCommerce.Platform.Core.PushNotifications ;
using VirtoCommerce.Platform.Core.Common;
using System.Collections.Concurrent;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
	[CLSCompliant(false)]
	public class ClientPushHub : Hub
	{
	}

	[CLSCompliant(false)]
	public class InMemoryPushNotificationManager : core.IPushNotificationManager
    {
		private ConcurrentBag<core.PushNotification> _innerList = new ConcurrentBag<core.PushNotification>();
		private readonly IHubContext _hubSignalR;

		public InMemoryPushNotificationManager(IHubContext hubSignalR)
		{
			_hubSignalR = hubSignalR;
		}
        #region INotifier Members

		public void Upsert(core.PushNotification notify)
        {
			if (notify == null)
			{
				throw new ArgumentNullException("notify");
			}

            var alreadyExistNotify = _innerList.FirstOrDefault(x => x.Id == notify.Id);

            if (alreadyExistNotify != null)
            {
                _innerList.TryTake(out alreadyExistNotify);
                _innerList.Add(notify);

            }
            else
            {
                var lastEvent = _innerList.OrderByDescending(x => x.Created).FirstOrDefault();
                if (lastEvent != null && lastEvent.ItHasSameContent(notify))
                {
                    lastEvent.IsNew = true;
                    lastEvent.RepeatCount++;
                    lastEvent.Created = DateTime.UtcNow;
                }
                else
                {
                    _innerList.Add(notify);
                }
            }
         
            _hubSignalR.Clients.All.notification(notify);

        }

		public core.PushNotificationSearchResult SearchNotifies(string userId, core.PushNotificationSearchCriteria criteria)
        {
            var query = _innerList.OrderByDescending(x => x.Created).Where(x => x.Creator == userId).AsQueryable();
			if (criteria.Ids != null && criteria.Ids.Any())
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

            if (criteria.OrderBy != null)
            {
                var parts = criteria.OrderBy.Split(':');
                if (parts.Count() > 0)
                {
                    var fieldName = parts[0];
                    if (parts.Count() > 1 && String.Equals(parts[1], "desc", StringComparison.InvariantCultureIgnoreCase))
                    {
                        query = query.OrderByDescending(fieldName);
                    }
                    else
                    {
                        query = query.OrderBy(fieldName);
                    }
                }
            }

			var retVal = new core.PushNotificationSearchResult
            {
                TotalCount = query.Count(),
                NewCount = query.Where(x => x.IsNew).Count(),
                NotifyEvents = query.Skip(criteria.Start).Take(criteria.Count).ToList()
            };

            return retVal;
        }

        #endregion
    }
}
