using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class MarketingEvent : IMarketingEvent
	{
		public string EventType { get; set;}
		public Dictionary<string, string> EventParams { get; set; }
	}
}
