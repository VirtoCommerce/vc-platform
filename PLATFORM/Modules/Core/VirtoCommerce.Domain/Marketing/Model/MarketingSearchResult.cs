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
			ContentFolders = new List<DynamicContentFolder>();
		}

		public int TotalCount { get; set; }

		public ICollection<Promotion> Promotions { get; set; }
		public ICollection<Coupon> Coupons { get; set; }
		public ICollection<DynamicContentPlace> ContentPlaces { get; set; }
		public ICollection<DynamicContentItem> ContentItems { get; set; }
		public ICollection<DynamicContentPublication> ContentPublications { get; set; }
		public ICollection<DynamicContentFolder> ContentFolders { get; set; }

	}
}
