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
	public class GiftReward : PromotionReward
	{
		public string Name { get; set; }

		public string CategoryId { get; set; }

		public string ProductId { get; set; }

		public int Quantity { get; set; }

		public string MeasureUnit { get; set; }

		public string ImageUrl { get; set; }

	}
}
