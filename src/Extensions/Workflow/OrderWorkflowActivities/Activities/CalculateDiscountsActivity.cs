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

		IPromotionUsageProvider _promotionUsageProvider;
		private IPromotionUsageProvider PromotionUsageProvider
		{
			get { return _promotionUsageProvider ?? (_promotionUsageProvider = ServiceLocator.GetInstance<IPromotionUsageProvider>()); }
		}

		IPricelistRepository _priceListRepository;
		private IPricelistRepository PriceListRepository
		{
			get { return _priceListRepository ?? (_priceListRepository = ServiceLocator.GetInstance<IPricelistRepository>()); }
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
			IPromotionUsageProvider promotionUsageProvider)
		{
			_catalogRepository = catalogRepository;
			_marketingRepository = marketingRepository;
			_promotionEntryPopulate = entryPopulate;
			_promotionUsageProvider = promotionUsageProvider;
			_priceListRepository = priceListRepository;
			_customerSessionService = customerService;
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
			var allRecords = EvaluatePolicies(recordsList.ToArray());

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
				var evaluator = new DefaultPromotionEvaluator(MarketingRepository, PromotionUsageProvider, new IEvaluationPolicy[] { new GlobalExclusivityPolicy(), new CartSubtotalRewardCombinePolicy(), new ShipmentRewardCombinePolicy() }, CacheRepository);
				var promotions = evaluator.EvaluatePromotion(evaluationContext);
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
					var evaluator = new DefaultPromotionEvaluator(MarketingRepository, PromotionUsageProvider,
						new IEvaluationPolicy[]
							{
								new GlobalExclusivityPolicy(), 
								new CartSubtotalRewardCombinePolicy(), 
								new ShipmentRewardCombinePolicy()
							}, CacheRepository);
					var promotions = evaluator.EvaluatePromotion(evaluationContext);
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

		private PromotionRecord[] EvaluatePolicies(PromotionRecord[] records)
		{
			var policies = new IEvaluationPolicy[] { new GlobalExclusivityPolicy(), new GroupExclusivityPolicy(), new CartSubtotalRewardCombinePolicy(), new ShipmentRewardCombinePolicy() };

			// make sure to sort the records correctly
			// 1st: items with a coupon applied
			// 2nd: catalog items
			// 3rd: order
			// 4th: shipping

			records = SortPromotionRecords(records);

			return policies.Aggregate(records, (current, policy) => policy.FilterPromotions(null, current));
		}

		private PromotionRecord[] SortPromotionRecords(PromotionRecord[] records)
		{
			var all = new List<PromotionRecord>();
			var recordsWithCoupons = from r in records where !String.IsNullOrEmpty(r.Reward.Promotion.CouponId) orderby r.Reward.Promotion.Priority descending select r;

			// all all coupon records first
			all.Add(recordsWithCoupons);

			var catalogRecords = from r in records where r.PromotionType == PromotionType.CatalogPromotion && !all.Contains(r) orderby r.Reward.Promotion.Priority descending select r;
			all.Add(catalogRecords);

			var cartRecords = from r in records where r.PromotionType == PromotionType.CartPromotion && !all.Contains(r) orderby r.Reward.Promotion.Priority descending select r;
			all.Add(cartRecords);

			return all.ToArray();
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
						discountAmount = orderSubTotal * reward.Amount * 0.01m;
						break;
					case (int)RewardAmountType.Absolute:
						discountAmount = reward.Amount;
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
						discountAmount = quantity * lineItem.PlacedPrice * reward.Amount * 0.01m;
					}
					else if (reward.AmountTypeId == (int)RewardAmountType.Absolute)
					{
						discountAmount = quantity * reward.Amount;
					}
					discountTotal += discountAmount;
					AddDiscountToEntity(lineItem, reward, discountAmount);
					limitQuantity -= quantity;
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

				var shipment = CurrentOrderGroup.OrderForms[0].Shipments.FirstOrDefault(x => x.ShippingMethodId == reward.ShippingMethodId);
				if (shipment != null)
				{
					var discountAmount = 0m;
					switch (reward.AmountTypeId)
					{
						case (int)RewardAmountType.Relative:
							discountAmount = shipment.Subtotal * reward.Amount * 0.01m;
							break;
						case (int)RewardAmountType.Absolute:
							discountAmount = reward.Amount;
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
						OrderFormId = orderForm.OrderFormId
					};
					orderForm.Discounts.Add(discount);
				}
				discount.DiscountAmount += discountAmount;
				// done in calculate total activity
				// orderForm.DiscountAmount = discount.DiscountAmount;

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
						LineItemId = lineItem.LineItemId
					};
					lineItem.Discounts.Add(discount);
				}
				discount.DiscountAmount += discountAmount;
				// done in calculate total activity
				// lineItem.LineItemDiscountAmount = discount.DiscountAmount;
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
						ShipmentId = shipment.ShipmentId
					};
					shipment.Discounts.Add(discount);
				}
				discount.DiscountAmount += discountAmount;
				// done in calculate total activity
				// shipment.ShippingDiscountAmount = discount.DiscountAmount;
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
					CatalogOutline = CatalogOutlineBuilder.BuildCategoryOutline(CatalogRepository, CustomerSessionService.CustomerSession.CatalogId, sku)
				};

			return retVal;
		}
	}
}
