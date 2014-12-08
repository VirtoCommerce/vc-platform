using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
	public interface IFeedbackCounterSupport
	{
		int PositiveFeedbackCount { get; set; }
		int NegativeFeedbackCount { get; set; }
		int AbuseCount { get; set; }
	}
}
