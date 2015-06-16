using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public interface INotificationTemplateResolver
	{
		void ResolveSubject(Notification notification);
		void ResolveBody(Notification notification);
	}
}
