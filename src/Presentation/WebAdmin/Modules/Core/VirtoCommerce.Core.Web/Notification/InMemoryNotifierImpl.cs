using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Framework.Web.Notification;

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
				if(notify.Description != null)
					alreadyExistNotify.Description = notify.Description;
				if (notify.Title != null)
					alreadyExistNotify.Title = notify.Title;

				alreadyExistNotify.New = notify.New;
				alreadyExistNotify.Status = notify.Status;
				if(notify.FinishDate != null)
					alreadyExistNotify.FinishDate = notify.FinishDate;
			}
			else
			{
				_innerList.Add(notify);
			}
		}

		public NotifySearchResult SearchNotifies(string userId, NotifySearchCriteria criteria)
		{
			var query = _innerList.Where(x=>x.Creator == userId).AsQueryable();
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

			var retVal = new NotifySearchResult
			{
				TotalCount = query.Count(),
				NewCount = query.Where(x=>x.New).Count(),
				NotifyEvents = query.OrderByDescending(x => x.Created).Skip(criteria.Start).Take(criteria.Count).ToList()
			};
			
			return retVal;
		}

		#endregion
	}
}
