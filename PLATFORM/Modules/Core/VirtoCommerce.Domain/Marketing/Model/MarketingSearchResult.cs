using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	public class MarketingSearchResult
	{
		public MarketingSearchResult()
		{
			Promotions = new List<Promotion>();
			Coupons = new List<Coupon>();
			ContentPlaces = new List<DynamicContentPlace>();
			ContentItems = new List<DynamicContentItem>();
			ContentPublications = new List<DynamicContentPublication>();
		}

		public int TotalCount { get; set; }

		public List<Promotion> Promotions { get; set; }
		public List<Coupon> Coupons { get; set; }
		public List<DynamicContentPlace> ContentPlaces { get; set; }
		public List<DynamicContentItem> ContentItems { get; set; }
		public List<DynamicContentPublication> ContentPublications { get; set; }

	}
}
