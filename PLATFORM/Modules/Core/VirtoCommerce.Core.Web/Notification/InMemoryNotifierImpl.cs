using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Platform.Core.Notification;

namespace VirtoCommerce.CoreModule.Web.Notification
{
	public class InMemoryNotifierImpl : INotifier
	{
		private List<NotifyEvent> _innerList = new List<NotifyEvent>();
		#region INotifier Members

		public void Upsert(NotifyEvent notify)
		{
			var alreadyExistNotify = _innerList.FirstOrDefault(x => x.Id == notify.Id);

			if(alreadyExistNotify != null)
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
		}

		public NotifySearchResult SearchNotifies(string userId, NotifySearchCriteria criteria)
		{
			var query = _innerList.OrderByDescending(x => x.Created).Where(x=>x.Creator == userId).AsQueryable();
			if(criteria.OnlyNew)
			{
				query = query.Where(x => x.New);
			}
			if(criteria.StartDate != null)
			{
				query = query.Where(x => x.Created >= criteria.StartDate);
			}
			if(criteria.EndDate != null)
			{
				query = query.Where(x => x.Created <= criteria.EndDate);
			}

			if(criteria.OrderBy != null)
			{
				var parts = criteria.OrderBy.Split(':');
				if(parts.Count() > 0)
				{
					var fieldName = parts[0];
					if(parts.Count() > 1 && String.Equals(parts[1], "desc", StringComparison.InvariantCultureIgnoreCase))
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
				NewCount = query.Where(x=>x.New).Count(),
				NotifyEvents = query.Skip(criteria.Start).Take(criteria.Count).ToList()
			};
			
			return retVal;
		}

		#endregion
	}
}
