using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model.DynamicContent
{
	public class DynamicContentEvaluationContext :  IDynamicContentEvaluationContext
	{
		public DynamicContentEvaluationContext()
		{
		}

		public DynamicContentEvaluationContext(string placeName, DateTime toDate, TagSet tags)
		{
			PlaceName = placeName;
			ToDate = toDate;
		    Tags = tags;
		}

		public string PlaceName { get; set; }

        public TagSet Tags { get; set; }

		public DateTime ToDate { get; set; }
	}
}
