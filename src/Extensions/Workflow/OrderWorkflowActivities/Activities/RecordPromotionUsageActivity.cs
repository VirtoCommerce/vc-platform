using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.OrderWorkflow
{
    public class RecordPromotionUsageActivity : OrderActivityBase
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

		ICustomerSessionService _customerSessionService;
		protected ICustomerSessionService CustomerSessionService
		{
			get
			{
				return _customerSessionService ??
					   (_customerSessionService = ServiceLocator.GetInstance<ICustomerSessionService>());
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

		public RecordPromotionUsageActivity()
		{
		}

        public RecordPromotionUsageActivity(ICatalogRepository catalogRepository,
			ICustomerSessionService customerService,
			IMarketingRepository marketingRepository)
		{
			_catalogRepository = catalogRepository;
			_marketingRepository = marketingRepository;
			_customerSessionService = customerService;
		}


        protected override void Execute(System.Activities.CodeActivityContext context)
        {
            base.Execute(context);

            if (ServiceLocator == null)
                return;

            if (CurrentOrderGroup == null || CurrentOrderGroup.OrderForms.Count == 0)
                return;

            var prop = context.DataContext.GetProperties().Find("IsCheckout", true);
            //This variable is used in ShoppingCartCheckoutWorkflow
            var isCheckout = false;

            if (prop != null && prop.PropertyType == typeof (bool))
            {
                var val = prop.GetValue(context.DataContext);
                isCheckout = val!=null && (bool)val;
            }

            var updateStatus = isCheckout ? PromotionUsageStatus.Used : PromotionUsageStatus.Reserved;

            var currentUsages = MarketingRepository.PromotionUsages.Where(p => p.OrderGroupId == CurrentOrderGroup.OrderGroupId).ToList();

            var usedPromotionIds = new List<string>();

            foreach (var orderForm in CurrentOrderGroup.OrderForms)
            {
                //create records for order form discounts
                usedPromotionIds.AddRange(orderForm.Discounts
                    .Select(formDiscount => UpdatePromotionUsage(currentUsages, formDiscount, updateStatus))
                    .Select(usage => usage.PromotionId));

                //create records for line item discounts
                usedPromotionIds.AddRange(orderForm.LineItems.SelectMany(x => x.Discounts)
                    .Select(lineItemDiscount => UpdatePromotionUsage(currentUsages, lineItemDiscount, updateStatus))
                    .Select(usage => usage.PromotionId));

                //create records for shipment discounts
                usedPromotionIds.AddRange(orderForm.Shipments.SelectMany(x => x.Discounts)
                   .Select(shipmentDiscount => UpdatePromotionUsage(currentUsages, shipmentDiscount, updateStatus))
                   .Select(usage => usage.PromotionId));
            }

            usedPromotionIds = usedPromotionIds.Distinct().ToList();

            //Expire all unused usages (they could be removed from cart)
            foreach (var unusedUsage in currentUsages.Where(x => !usedPromotionIds.Contains(x.PromotionId)))
            {
                unusedUsage.Status = (int)PromotionUsageStatus.Expired;
            }

            MarketingRepository.UnitOfWork.Commit();
        }

        private PromotionUsage UpdatePromotionUsage(ICollection<PromotionUsage> currentUsages, Discount discount, PromotionUsageStatus status)
        {
            var usage = currentUsages.FirstOrDefault(x => x.PromotionId == discount.PromotionId);

            if (usage != null)
            {
                usage.Status = (int)status;
                usage.UsageDate = DateTime.UtcNow;
            }
            else
            {
                usage = new PromotionUsage
                {
                    CouponCode = discount.DiscountCode,
                    MemberId = CustomerSessionService.CustomerSession.CustomerId,
                    OrderGroupId = CurrentOrderGroup.OrderGroupId,
                    PromotionId = discount.PromotionId,
                    Status = (int)status,
                    UsageDate = DateTime.UtcNow
                };

                //Need to add here too to avoid duplicates
                currentUsages.Add(usage);

                MarketingRepository.Add(usage);
            }

            return usage;
        }
	}
}
