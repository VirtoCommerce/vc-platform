using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Platform.Data.Notification
{
	[CLSCompliant(false)]
	public class ClientPushHub : Hub
	{
	}

	[CLSCompliant(false)]
    public class InMemoryNotifierImpl : INotifier
    {
        private List<NotifyEvent> _innerList = new List<NotifyEvent>();
		private readonly IHubContext _hubSignalR;

		public InMemoryNotifierImpl(IHubContext hubSignalR)
		{
			_hubSignalR = hubSignalR;
		}
        #region INotifier Members

        public void Upsert(NotifyEvent notify)
        {
            var alreadyExistNotify = _innerList.FirstOrDefault(x => x.Id == notify.Id);

            if (alreadyExistNotify != null)
            {
                _innerList.Remove(alreadyExistNotify);
                _innerList.Add(notify);

            }
            else
            {
                var lastEvent = _innerList.OrderByDescending(x => x.Created).FirstOrDefault();
                if (lastEvent != null && lastEvent.ItHasSameContent(notify))
                {
                    lastEvent.New = true;
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

        public NotifySearchResult SearchNotifies(string userId, NotifySearchCriteria criteria)
        {
            var query = _innerList.OrderByDescending(x => x.Created).Where(x => x.Creator == userId).AsQueryable();
			if (criteria.Ids != null && criteria.Ids.Any())
			{
				query = query.Where(x => criteria.Ids.Contains(x.Id));
			}
            if (criteria.OnlyNew)
            {
                query = query.Where(x => x.New);
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

            var retVal = new NotifySearchResult
            {
                TotalCount = query.Count(),
                NewCount = query.Where(x => x.New).Count(),
                NotifyEvents = query.Skip(criteria.Start).Take(criteria.Count).ToList()
            };

            return retVal;
        }

        #endregion
    }
}
