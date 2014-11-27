using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Framework.Web.Notification
{
	public interface INotifier
	{
		NotifyEvent Create(NotifyEvent notify);
		void Update(NotifyEvent notify);
		NotifySearchResult SearchNotifies(string userId, NotifySearchCriteria criteria);

	}
}
