using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
	public class Review : IFeedbackCounterSupport
	{
		public string Id { get; set; }
		
        public int Rating { get; set; }
        public string RatingComment { get; set; }
	
        public DateTime? Created { get; set; }

		public string Author { get; set; }
	
		public string ReviewText { get; set; }
	
        public ICollection<ReviewComment> Comments { get; set; }
		
        public int TotalComments { get; set; }

		#region IFeedbackCounterSupport
		public int PositiveFeedbackCount { get; set; }
		public int NegativeFeedbackCount { get; set; }
		public int AbuseCount { get; set; }
		#endregion
	
	}
}
