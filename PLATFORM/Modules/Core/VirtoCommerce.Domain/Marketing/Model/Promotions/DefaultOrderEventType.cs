using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public enum OrderEventType
	{
		OrderCancelEvent,
		OrderCompleteEvent,
		OrderPaidEvent, 
		OrderPlacedEvent,
		CustomerLoginEvent,
		CustomerRegisterEvent
	}
}
