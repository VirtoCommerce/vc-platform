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
		public GiftReward()
		{
		}

		//Copy constructor
		protected GiftReward(GiftReward other)
			:base(other)
		{
			Name = other.Name;
			CategoryId = other.CategoryId;
			ProductId = other.ProductId;
			Quantity = other.Quantity;
			MeasureUnit = other.MeasureUnit;
			ImageUrl = other.ImageUrl;
		}

		public string Name { get; set; }

		public string CategoryId { get; set; }

		public string ProductId { get; set; }

		public int Quantity { get; set; }

		public string MeasureUnit { get; set; }

		public string ImageUrl { get; set; }

		public override PromotionReward Clone()
		{
			return new GiftReward(this);
		}

		public override string ToString()
		{
			return String.Format("{0} {1}", base.ToString(), Name);
		}
	}
}
