using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model.Policies
{
	public class ShipmentRewardCombinePolicy : IEvaluationPolicy
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
			var shipmentRewards = retVal.SelectMany(x => x.Rewards.OfType<ShipmentReward>()).ToArray();
			//Only a max relative(%) discount or absolute($). The relative (%) has a exlude the absolute ($).
			var maxShipmentReward = shipmentRewards.Where(x => x.AmountTypeId == (int)RewardAmountType.Relative)
																   .OrderByDescending(x => x.Amount)
																   .FirstOrDefault();
			if (maxShipmentReward == null)
			{
				maxShipmentReward = shipmentRewards.Where(x => x.AmountTypeId == (int)RewardAmountType.Absolute)
																	   .OrderByDescending(x => x.Amount)
																	   .FirstOrDefault();
			}
			if (maxShipmentReward != null)
			{
				//Remove all other shipment rewards from promotions
				foreach (var toRemoveReward in shipmentRewards.Where(x => x != maxShipmentReward))
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
