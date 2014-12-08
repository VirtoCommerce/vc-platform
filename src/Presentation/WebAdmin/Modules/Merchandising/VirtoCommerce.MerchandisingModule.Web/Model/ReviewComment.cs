using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
	public class ReviewComment : IFeedbackCounterSupport
	{
		public string Id { get; set; }
		public string Author { get; set; }
		public string Comment { get; set; }
	
		public DateTime? CreatedDateTime { get; set; }


		#region IFeedbackCounterSupport
		public int PositiveFeedbackCount { get; set; }

		public int NegativeFeedbackCount { get; set; }

		public int AbuseCount { get; set; }
		#endregion
	}
}
