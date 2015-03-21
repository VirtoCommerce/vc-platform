using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Framework.Web.Notification
{
	public interface INotifier
	{
		void Upsert(NotifyEvent notify);
		NotifySearchResult SearchNotifies(string userId, NotifySearchCriteria criteria);

	}
}
