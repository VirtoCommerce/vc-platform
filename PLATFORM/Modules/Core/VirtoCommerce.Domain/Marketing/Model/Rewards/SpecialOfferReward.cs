using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Marketing.Model
{
	/// <summary>
	/// Special offer reward
	/// </summary>
	public class SpecialOfferReward : PromotionReward
	{
		public SpecialOfferReward()
		{
		}
		//Copy constructor
		protected SpecialOfferReward(SpecialOfferReward other)
			:base(other)
		{
		}
		public override PromotionReward Clone()
		{
			return new SpecialOfferReward(this);
		}
	}
}
