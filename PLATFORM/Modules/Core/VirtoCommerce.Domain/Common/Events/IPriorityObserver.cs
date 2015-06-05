using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Common.Events
{
	public interface IPriorityObserver
	{
		int Priority { get; }
	}
}
