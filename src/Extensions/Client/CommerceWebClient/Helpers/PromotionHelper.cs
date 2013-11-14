using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Client;

namespace VirtoCommerce.Web.Client.Helpers
{
    /// <summary>
    /// Class PromotionHelper.
    /// </summary>
	public class PromotionHelper
	{
        /// <summary>
        /// Gets the promotion client.
        /// </summary>
        /// <value>The promotion client.</value>
		public static PromotionClient PromotionClient
		{
			get
			{
				return ServiceLocator.Current.GetInstance<PromotionClient>();
			}
		}

        /// <summary>
        /// Determines whether the specified item identifier is promotion.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="promotionId">The promotion identifier.</param>
        /// <returns><c>true</c> if the specified item identifier is promotion; otherwise, <c>false</c>.</returns>
		public static bool IsPromotion(string itemId, out string promotionId)
		{
			return PromotionClient.IsSkuItemPromotion(itemId, out promotionId);
		}
	}
}
