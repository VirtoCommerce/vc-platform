using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
    public class PushNotificationManager : IPushNotificationManager
    {
        private readonly List<PushNotification> _innerList = new List<PushNotification>();
        private object _lockObject = new object();
        private readonly IHubContext<PushNotificationHub> _hubContext;
        public PushNotificationManager(IHubContext<PushNotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria)
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

            var sortInfos = criteria.SortInfos;
            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<PushNotification>(x => x.Created), SortDirection = SortDirection.Descending } };
            }

            var retVal = new PushNotificationSearchResult
            {
                TotalCount = query.Count(),
                NewCount = query.Count(x => x.IsNew),
                NotifyEvents = query.Skip(criteria.Skip).Take(criteria.Take).ToList()
            };

            return retVal;
        }

        public void Send(PushNotification notification)
        {
            SendAsync(notification).GetAwaiter().GetResult();
        }

        public async Task SendAsync(PushNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            lock (_lockObject)
            {
                var alreadyExistNotify = _innerList.FirstOrDefault(x => x.Id == notification.Id);
                if (alreadyExistNotify != null)
                {
                    _innerList.Remove(alreadyExistNotify);
                    _innerList.Add(notification);
                }
                else
                {
                    var lastEvent = _innerList.OrderByDescending(x => x.Created).FirstOrDefault();
                    if (lastEvent != null && lastEvent.ItHasSameContent(notification))
                    {
                        lastEvent.IsNew = true;
                        lastEvent.RepeatCount++;
                        lastEvent.Created = DateTime.UtcNow;
                    }
                    else
                    {
                        _innerList.Add(notification);
                    }
                }
            }

            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendCoreAsync("Send", new[] { notification });
            }
        }

    }

}
