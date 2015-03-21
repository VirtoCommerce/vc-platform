using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Model.Policies;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Model;
using cust = VirtoCommerce.Foundation.Customers.Services;

namespace VirtoCommerce.OrderWorkflow
{
    using VirtoCommerce.Foundation.Catalogs.Services;

    using IEvaluationPolicy = VirtoCommerce.Foundation.Marketing.Model.IEvaluationPolicy;

    /// <summary>
	/// Calculates and adds discounts to the current order. Discounts included are: catalog, order and shipping.
	/// </summary>
	public class CalculateDiscountsActivity : OrderActivityBase
	{

		ICatalogRepository _catalogRepository;
		protected ICatalogRepository CatalogRepository
		{
			get { return _catalogRepository ?? (_catalogRepository = ServiceLocator.GetInstance<ICatalogRepository>()); }
			set
			{
				_catalogRepository = value;
			}
		}

		cust.ICustomerSessionService _customerSessionService;
		protected cust.ICustomerSessionService CustomerSessionService
		{
			get
			{
				return _customerSessionService ??
					   (_customerSessionService = ServiceLocator.GetInstance<cust.ICustomerSessionService>());
			}
			set
			{
				_customerSessionService = value;
			}
		}

		IMarketingRepository _marketingRepository;
		protected IMarketingRepository MarketingRepository
		{
			get { return _marketingRepository ?? (_marketingRepository = ServiceLocator.GetInstance<IMarketingRepository>()); }
			set
			{
				_marketingRepository = value;
			}
		}

		IPromotionEntryPopulate _promotionEntryPopulate;
		private IPromotionEntryPopulate PromotionEntryPopulate
		{
			get { return _promotionEntryPopulate ?? (_promotionEntryPopulate = ServiceLocator.GetInstance<IPromotionEntryPopulate>()); }
		}

		IPricelistRepository _priceListRepository;
		private IPricelistRepository PriceListRepository
		{
			get { return _priceListRepository ?? (_priceListRepository = ServiceLocator.GetInstance<IPricelistRepository>()); }
		}

        ICatalogOutlineBuilder _catalogOutlineBuilder;

        IPromotionEvaluator _promotionEvaluator;
        protected IPromotionEvaluator PromotionEvaluator
        {
            get { return _promotionEvaluator ?? (_promotionEvaluator = ServiceLocator.GetInstance<IPromotionEvaluator>()); }
            set
            {
                _promotionEvaluator = value;
            }
        }

        private ICatalogOutlineBuilder OutlineBuilder
        {
            get { return _catalogOutlineBuilder ?? (_catalogOutlineBuilder = ServiceLocator.GetInstance<ICatalogOutlineBuilder>()); }
        }

		private ICacheRepository CacheRepository
		{
			get
			{
				return ServiceLocator.GetInstance<ICacheRepository>();
			}
		}

		public CalculateDiscountsActivity()
		{
		}

		public CalculateDiscountsActivity(ICatalogRepository catalogRepository,
			cust.ICustomerSessionService customerService,
			IMarketingRepository marketingRepository,
			IPricelistRepository priceListRepository,
			IPromotionEntryPopulate entryPopulate,
            ICatalogOutlineBuilder catalogOutlineBuilder,
            IPromotionEvaluator evaluator)
		{
			_catalogRepository = catalogRepository;
			_marketingRepository = marketingRepository;
			_promotionEntryPopulate = entryPopulate;
			_priceListRepository = priceListRepository;
			_customerSessionService = customerService;
		    _catalogOutlineBuilder = catalogOutlineBuilder;
            _promotionEvaluator = evaluator;
		}

		protected override void Execute(System.Activities.CodeActivityContext context)
		{
			base.Execute(context);

			if (ServiceLocator == null)
				return;

			if (CurrentOrderGroup == null || CurrentOrderGroup.OrderForms.Count == 0)
				return;


			// get line item promotions
			var lineItemRecords = CalculateLineItemDiscounts();

			// get cart promotions
			var cartRecords = CalculateCartDiscounts();

			// combine all records
			var recordsList = new List<PromotionRecord>(lineItemRecords);
			recordsList.Add(cartRecords);

			// filter policies
			var allRecords = PromotionEvaluator.EvaluatePolicies(recordsList.ToArray());

			//3. Apply discounts
			var cartSubtotalRewards = (from r in allRecords where r.Reward is CartSubtotalReward select r).ToArray();
			var lineItemRewards = (from r in allRecords where r.Reward is CatalogItemReward select r).ToArray();
			var shipmentRewards = (from r in allRecords where r.Reward is ShipmentReward select r).ToArray();

			var lineItemDiscountTotal = ApplyCatalogItemRewards(lineItemRewards);
			var orderSubTotal = CurrentOrderGroup.Subtotal - lineItemDiscountTotal;
			//Apply order subtotal discounts
			ApplyOrderSubtotalReward(orderSubTotal, cartSubtotalRewards);
			//Apply shipment discounts
			ApplyShipmentReward(shipmentRewards);
			//Add Gifts
			ApplyGiftReward(null, lineItemRewards);
		}

		private IEnumerable<PromotionRecord> CalculateCartDiscounts()
		{
			var records = new List<PromotionRecord>();
			foreach (var form in CurrentOrderGroup.OrderForms)
			{
				var set = GetPromotionEntrySetFromOrderForm(form);

				// create context 
				var ctx = new Dictionary<string, object> { { PromotionEvaluationContext.TargetSet, set } };

				var couponCode = CustomerSessionService.CustomerSession.CouponCode;

				//1. Prepare marketing context
				var evaluationContext = new PromotionEvaluationContext
				{
					ContextObject = ctx,
					CustomerId = CustomerSessionService.CustomerSession.CustomerId,
					CouponCode = CustomerSessionService.CustomerSession.CouponCode,
					PromotionType = PromotionType.CartPromotion,
					Currency = CustomerSessionService.CustomerSession.Currency,
					Store = CustomerSessionService.CustomerSession.StoreId,
					IsRegisteredUser = CustomerSessionService.CustomerSession.IsRegistered,
					IsFirstTimeBuyer = CustomerSessionService.CustomerSession.IsFirstTimeBuyer
				};

				//2. Evaluate 
				//var evaluator = new DefaultPromotionEvaluator(MarketingRepository, PromotionUsageProvider, new IEvaluationPolicy[] { new GlobalExclusivityPolicy(), new CartSubtotalRewardCombinePolicy(), new ShipmentRewardCombinePolicy() }, CacheRepository);
				var promotions = PromotionEvaluator.EvaluatePromotion(evaluationContext);
				var rewards = promotions.SelectMany(x => x.Rewards);

				//3. Generate warnings
				if (!string.IsNullOrEmpty(couponCode) && !promotions.Any(p => (p.CouponId != null && p.Coupon.Code == couponCode)
					|| (p.CouponSetId != null && p.CouponSet.Coupons.Any(c => c.Code == couponCode))))
				{
					RegisterWarning(WorkflowMessageCodes.COUPON_NOT_APPLIED, "Coupon doesn't exist or is not applied");
				}

				records.AddRange(rewards.Select(reward => new PromotionRecord
					{
						AffectedEntriesSet = set,
						TargetEntriesSet = set,
						Reward = reward,
						PromotionType = PromotionType.CartPromotion
					}));
			}

			return records.ToArray();
		}

		private IEnumerable<PromotionRecord> CalculateLineItemDiscounts()
		{
			var records = new List<PromotionRecord>();
			foreach (var form in CurrentOrderGroup.OrderForms)
			{
				foreach (var lineItem in form.LineItems)
				{
					var set = new PromotionEntrySet();
					var entry = GetPromotionEntryFromLineItem(lineItem);
					set.Entries.Add(entry);

					// create context 
					var ctx = new Dictionary<string, object> { { PromotionEvaluationContext.TargetSet, set } };

					//1. Prepare marketing context
					var evaluationContext = new PromotionEvaluationContext
						{
							ContextObject = ctx,
							CustomerId = CustomerSessionService.CustomerSession.CustomerId,
							CouponCode = CustomerSessionService.CustomerSession.CouponCode,
							Currency = CustomerSessionService.CustomerSession.Currency,
							PromotionType = PromotionType.CatalogPromotion,
							Store = CustomerSessionService.CustomerSession.StoreId,
							IsRegisteredUser = CustomerSessionService.CustomerSession.IsRegistered,
							IsFirstTimeBuyer = CustomerSessionService.CustomerSession.IsFirstTimeBuyer
						};

					//2. Evaluate 
					var promotions = PromotionEvaluator.EvaluatePromotion(evaluationContext);
					var rewards = promotions.SelectMany(x => x.Rewards);

					records.AddRange(rewards.Select(reward => new PromotionRecord
						{
							AffectedEntriesSet = set,
							TargetEntriesSet = set,
							Reward = reward,
							PromotionType = PromotionType.CatalogPromotion
						}));
				}
			}

			return records.ToArray();
		}


		private PromotionEntrySet GetPromotionEntrySetFromOrderForm(OrderForm form)
		{
			var set = new PromotionEntrySet();

			foreach (var lineItem in form.LineItems)
			{
				var entry = GetPromotionEntryFromLineItem(lineItem);
				set.Entries.Add(entry);
			}

			return set;
		}

		private PromotionEntry GetPromotionEntryFromLineItem(LineItem lineItem)
		{
			var populate = PromotionEntryPopulate;
			var entry = new PromotionEntry(lineItem.Catalog, lineItem.CatalogItemId, lineItem.ExtendedPrice / lineItem.Quantity);
			populate.Populate(ref entry, lineItem);
			return entry;
		}

		private LineItem[] GetLineItemsSuitableForCatalogItemReward(PromotionRecord record)
		{
			var reward = record.Reward as CatalogItemReward;
			if (reward == null)
			{
				return new LineItem[0];
			}
			var entries = record.AffectedEntriesSet.Entries.ExcludeCategories(ParseStringIds(reward.ExcludingCategories))
								.ExcludeProducts(ParseStringIds(reward.ExcludingProducts))
								.ExcludeSkus(ParseStringIds(reward.ExcludingSkus))
								.InCategory(reward.CategoryId)
								.InProduct(reward.ProductId)
								.OfSku(reward.SkuId)
								.ToArray();

			var ids = from e in entries select e.EntryId;
			return (from l in CurrentOrderGroup.OrderForms[0].LineItems
					where ids.Contains(l.CatalogItemId, StringComparer.OrdinalIgnoreCase)
					select l).ToArray();
		}

		private void ApplyGiftReward(IEnumerable<string> rejectedGiftIds, IEnumerable<PromotionRecord> records)
		{
			// check if we need to skip adding new promotion items (like compare lists and wish lists)
			var session = CustomerSessionService.CustomerSession;
			var skipRewards = session["SkipRewards"];
			if (skipRewards != null && (bool)skipRewards)
			{
				return;
			}

			var giftRewards = (from r in records where r.Reward.AmountTypeId == (int)RewardAmountType.Gift select r.Reward).OfType<CatalogItemReward>();
			if (rejectedGiftIds != null)
			{
				giftRewards = giftRewards.Where(x => !rejectedGiftIds.Contains(x.SkuId, StringComparer.OrdinalIgnoreCase));
			}
			foreach (var giftReward in giftRewards)
			{
				var giftSku = CatalogRepository.Items.FirstOrDefault(x => x.ItemId == giftReward.SkuId) as Sku;
				if (giftSku != null)
				{
					var giftQuantity = giftReward.QuantityLimit;
					if (giftQuantity > 0)
					{
						var giftLineItem = Sku2LineItem(giftSku, giftQuantity);
						if (giftLineItem != null)
						{
							giftLineItem.OrderFormId = CurrentOrderGroup.OrderForms[0].OrderFormId;
							CurrentOrderGroup.OrderForms[0].LineItems.Add(giftLineItem);
							AddDiscountToEntity(giftLineItem, giftReward, giftLineItem.PlacedPrice * giftQuantity);
						}
					}
				}
			}
		}

		private void ApplyOrderSubtotalReward(decimal orderSubTotal, IEnumerable<PromotionRecord> records)
		{
			foreach (var record in records)
			{
				var reward = record.Reward as CartSubtotalReward;
				if (reward == null)
					continue;

				var discountAmount = 0m;
				switch (reward.AmountTypeId)
				{
					case (int)RewardAmountType.Relative:
						discountAmount = Math.Round(orderSubTotal * reward.Amount * 0.01m,2);
						break;
					case (int)RewardAmountType.Absolute:
						discountAmount = Math.Round(reward.Amount,2);
						break;
				}
				AddDiscountToEntity(CurrentOrderGroup.OrderForms[0], reward, discountAmount);
			}
		}

		private decimal ApplyCatalogItemRewards(IEnumerable<PromotionRecord> records)
		{
			var discountTotal = 0m;

			foreach (var record in records)
			{
				var reward = record.Reward as CatalogItemReward;

				if (reward == null)
					continue;

				var lineItems = GetLineItemsSuitableForCatalogItemReward(record);

				//distribute discount amount between lineItems limit quantity


				var limitQuantity = reward.QuantityLimit == 0 ? lineItems.Sum(x => x.Quantity) : reward.QuantityLimit;
				foreach (var lineItem in lineItems)
				{
					var quantity = Math.Min(limitQuantity, lineItem.Quantity);
					var discountAmount = 0m;
					if (reward.AmountTypeId == (int)RewardAmountType.Relative)
					{
						discountAmount = Math.Round(quantity * lineItem.PlacedPrice * reward.Amount * 0.01m,2);
					}
					else if (reward.AmountTypeId == (int)RewardAmountType.Absolute)
					{
						discountAmount = Math.Round(quantity * reward.Amount,2);
					}
					discountTotal += discountAmount;
					AddDiscountToEntity(lineItem, reward, discountAmount);
					limitQuantity -= quantity;

                    //If we have reached limit there are no discounts left to apply
				    if (limitQuantity == 0) break;
				}
			}
			//TODO distribute discount amount between lineItems limit number positions of specific product
			return discountTotal;
		}

		private void ApplyShipmentReward(IEnumerable<PromotionRecord> records)
		{
			foreach (var record in records)
			{
				var reward = record.Reward as ShipmentReward;
				if (reward == null)
					continue;

				var shipment = CurrentOrderGroup.OrderForms[0].Shipments.FirstOrDefault(x => string.IsNullOrEmpty(x.ShippingMethodId) 
                    || x.ShippingMethodId == reward.ShippingMethodId);

				if (shipment != null)
				{
					var discountAmount = 0m;
					switch (reward.AmountTypeId)
					{
						case (int)RewardAmountType.Relative:
                            discountAmount = Math.Round(shipment.ShippingCost * reward.Amount * 0.01m, 2);
							break;
						case (int)RewardAmountType.Absolute:
                            discountAmount = Math.Round(reward.Amount, 2);
							break;
					}
					AddDiscountToEntity(shipment, reward, discountAmount);
				}
			}
		}

		private void AddDiscountToEntity(StorageEntity entity, PromotionReward reward, decimal discountAmount)
		{
			var orderForm = entity as OrderForm;
			var lineItem = entity as LineItem;
			var shipment = entity as Shipment;

			if (orderForm != null && reward is CartSubtotalReward)
			{
				var discount = orderForm.Discounts.FirstOrDefault(x => x.PromotionId == reward.PromotionId);
				if (discount == null)
				{
					discount = new OrderFormDiscount
					{
						PromotionId = reward.PromotionId,
						DiscountName = reward.Promotion.Name,
						DisplayMessage = reward.Promotion.Description,
						OrderFormId = orderForm.OrderFormId,
                        DiscountCode = reward.Promotion.Coupon !=null ? reward.Promotion.Coupon.Code : null
					};
					orderForm.Discounts.Add(discount);
				}
				discount.DiscountAmount += discountAmount;
			}
			else if (lineItem != null && reward is CatalogItemReward)
			{
				var discount = lineItem.Discounts.FirstOrDefault(x => x.PromotionId == reward.PromotionId);
				if (discount == null)
				{
					discount = new LineItemDiscount
					{
						PromotionId = reward.PromotionId,
						DiscountName = reward.Promotion.Name,
						DisplayMessage = reward.Promotion.Description,
						LineItemId = lineItem.LineItemId,
                        DiscountCode = reward.Promotion.Coupon != null ? reward.Promotion.Coupon.Code : null
					};
					lineItem.Discounts.Add(discount);
				}
				discount.DiscountAmount += discountAmount;
			}
			else if (shipment != null && reward is ShipmentReward)
			{
				var discount = shipment.Discounts.FirstOrDefault(x => x.PromotionId == reward.PromotionId);
				if (discount == null)
				{
					discount = new ShipmentDiscount
					{
						PromotionId = reward.PromotionId,
						DiscountName = reward.Promotion.Name,
						DisplayMessage = reward.Promotion.Description,
						ShipmentId = shipment.ShipmentId,
                        DiscountCode = reward.Promotion.Coupon != null ? reward.Promotion.Coupon.Code : null
					};
					shipment.Discounts.Add(discount);
				}
				discount.DiscountAmount += discountAmount;
			}
		}

		private string[] ParseStringIds(string serializedIds)
		{
			var retVal = serializedIds != null ? serializedIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };
			return retVal;
		}

		private LineItem Sku2LineItem(Item sku, decimal quantity)
		{
			var session = CustomerSessionService.CustomerSession;
			var price = PriceListRepository.FindLowestPrice(session != null ? session.Pricelists : null, sku.ItemId, quantity);
			var retVal = new LineItem
				{
					CatalogItemId = sku.ItemId,
					CatalogItemCode = sku.Code,
					DisplayName = sku.Name,
					Quantity = quantity,
					Catalog = sku.CatalogId,
					MaxQuantity = sku.MaxQuantity,
					MinQuantity = sku.MinQuantity,
					PlacedPrice = price != null ? (price.Sale ?? price.List) : 0,
					ListPrice = price != null ? (price.Sale ?? price.List) : 0,
                    CatalogOutline = OutlineBuilder.BuildCategoryOutline(CustomerSessionService.CustomerSession.CatalogId, sku.ItemId).ToString()
				};

			return retVal;
		}
	}
}
