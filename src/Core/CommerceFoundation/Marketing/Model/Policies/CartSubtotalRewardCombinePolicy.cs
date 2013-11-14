using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model.Policies
{
	/// <summary>
	/// The combination of discounts CartSubTotalReward
	/// % Discount can not be combined, counted off with the greatest value.
	/// $ Discount can not be combined, counted off from the highest value ..
	/// % Discount and $ can not be combined, only the% discount with the greatest value. 
	/// </summary>
	public class CartSubtotalRewardCombinePolicy : IEvaluationPolicy
	{
		#region IEvaluationPolicy Members

		public int Priority
		{
			get;
			set;
		}

		public PromotionRecord[] FilterPromotions(IPromotionEvaluationContext evaluationContext, PromotionRecord[] records)
		{
            /*
			var retVal = promotions;
			var cartSubtotalRewards = retVal.SelectMany(x => x.Rewards.OfType<CartSubtotalReward>()).ToArray();
			//Only a max relative(%) discount or absolute($). The relative (%) has a exlude the absolute ($).
			var maxCartSubtotalReward = cartSubtotalRewards.Where(x => x.AmountTypeId == (int)RewardAmountType.Relative)
																   .OrderByDescending(x => x.Amount)
																   .FirstOrDefault();
			if (maxCartSubtotalReward == null)
			{
				maxCartSubtotalReward = cartSubtotalRewards.Where(x => x.AmountTypeId == (int)RewardAmountType.Absolute)
																	   .OrderByDescending(x => x.Amount)
																	   .FirstOrDefault();
			}
			if (maxCartSubtotalReward != null)
			{
				//Remove all other cartSubtotalRewards from promotions
				foreach (var toRemoveReward in cartSubtotalRewards.Where(x => x != maxCartSubtotalReward))
				{
					toRemoveReward.Promotion.Rewards.Remove(toRemoveReward);
				}
			}

			return retVal.Where(x => x.Rewards.Count() > 0).ToArray();
             * */
            return records;
		}

		#endregion


        public string Group
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
