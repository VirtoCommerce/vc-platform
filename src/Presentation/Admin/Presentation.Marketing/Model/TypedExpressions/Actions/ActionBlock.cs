using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.ViewModel;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
    [Serializable]
	public class ActionBlock : PromotionExpressionBlock
    {
		public ActionBlock(string name, IExpressionViewModel promotionViewModel)
            : base(name, promotionViewModel)
        {
            WithLabel(name);
        }

        public override PromotionReward[] GetPromotionRewards()
        {
            List<PromotionReward> retVal = new List<PromotionReward>();
            foreach (var adaptor in this.Children.OfType<IExpressionMarketingAdaptor>())
            {
                retVal.AddRange(adaptor.GetPromotionRewards());
            }
            return retVal.ToArray();
        }
    }
}
