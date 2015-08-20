using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;

namespace VirtoCommerce.Domain.Marketing.Model.DynamicContent
{
	public class DynamicContentEvaluationContext :  EvaluationContextBase
	{
		public DynamicContentEvaluationContext()
		{
		}

		public DynamicContentEvaluationContext(string storeId, string placeName, DateTime toDate, TagSet tags)
		{
			StoreId = storeId;
			PlaceName = placeName;
			ToDate = toDate;
		    Tags = tags;
		}

		public string StoreId { get; set; }

		public string PlaceName { get; set; }

        public TagSet Tags { get; set; }

		public DateTime ToDate { get; set; }



	}
}
