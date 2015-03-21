using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.OrderWorkflow
{
    /// <summary>
    /// Removes all the discounts from the order. Should be run just before CalculateDiscountsActivity.
    /// </summary>
	public class RemoveDiscountActivity : OrderActivityBase
	{

		IMarketingRepository _marketingRepository;
		protected IMarketingRepository MarketingRepository
		{
			get { return _marketingRepository ?? (_marketingRepository = ServiceLocator.GetInstance<IMarketingRepository>()); }
			set
			{
				_marketingRepository = value;
			}
		}

		public RemoveDiscountActivity()
		{
			
		}

		public RemoveDiscountActivity(IMarketingRepository marketingRepository)
		{
			_marketingRepository = marketingRepository;
		}

	    protected override void Execute(System.Activities.CodeActivityContext context)
		{
			base.Execute(context);

            if (CurrentOrderGroup.OrderForms.Count == 0)
                return;

			var orderForm = CurrentOrderGroup.OrderForms[0];
            orderForm.Discounts.Clear();
			orderForm.DiscountAmount = 0m;


		    var lineItemsToRemove = new List<string>();
		    foreach (var lineItem in orderForm.LineItems.ToArray())
			{
				var reward = MarketingRepository.PromotionRewards.OfType<CatalogItemReward>().FirstOrDefault(pr => pr.SkuId == lineItem.CatalogItemId);

				//Line items that were added as result of promotion have to be removed
				if (reward != null && lineItem.Discounts.Any(x => x.PromotionId == reward.PromotionId))
				{
					orderForm.LineItems.Remove(lineItem);
					lineItemsToRemove.Add(lineItem.LineItemId);
				}
				else
				{
					lineItem.Discounts.Clear();
					lineItem.LineItemDiscountAmount = 0m;
				}
			}

			foreach (var shipment in orderForm.Shipments)
			{
                shipment.Discounts.Clear();
				shipment.ShippingDiscountAmount = 0m;

				var links = shipment.ShipmentItems.Where(link => lineItemsToRemove.Contains(link.LineItemId)).ToList();

				foreach (var link in links)
				{
					shipment.ShipmentItems.Remove(link);
				}
			}

            //Remove usages for current orderGroup.
            var usages = MarketingRepository.PromotionUsages.Where(x => x.OrderGroupId == CurrentOrderGroup.OrderGroupId && x.Status != (int)PromotionUsageStatus.Used).ToList();

            foreach (var promotionUsage in usages)
            {
                MarketingRepository.Remove(promotionUsage);
            }

            //Must clear them before discounts are applied. Otherwise discounts will not be applied correctly
	        MarketingRepository.UnitOfWork.Commit();
		}
	}
}
