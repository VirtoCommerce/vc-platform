using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	/// <summary>
	/// Gift
	/// </summary>
	public class GiftCatalogItemReward : PromotionReward
	{

		public string Name { get; set; }


		public string CategoryId { get; set; }

		public string SellerId { get; set; }


		public string ProductId { get; set; }


		public int Quantity { get; set; }

		public string Unit { get; set; }
		
		public string HtmlInformer { get; set; }

		public string ThumbImageUrl { get; set; }

		public string OriginalImageUrl { get; set; }

		public string Description { get; set; }
	}
}
