using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.OrderWorkflow
{
    public class RecordPromotionUsageActivity : OrderActivityBase
    {
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

        public PromotionUsageStatus UsageStatus { get; set; }

		public RecordPromotionUsageActivity()
		{
		}

        public RecordPromotionUsageActivity(ICustomerSessionService customerService, IMarketingRepository marketingRepository)
		{
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

            //Remove usages for current orderGroup
            var usages = MarketingRepository.PromotionUsages.Where(x => x.OrderGroupId == CurrentOrderGroup.OrderGroupId && x.Status != (int)PromotionUsageStatus.Used).ToList();

            foreach (var promotionUsage in usages)
            {
                MarketingRepository.Remove(promotionUsage);
            }

            var currentUsages = new List<PromotionUsage>();

            foreach (var orderForm in CurrentOrderGroup.OrderForms)
            {
                //create records for order form discounts
                orderForm.Discounts.ToList().ForEach(formDiscount => UpdatePromotionUsage(currentUsages, formDiscount));

                //create records for line item discounts
                orderForm.LineItems.SelectMany(x => x.Discounts).ToList().ForEach(lineItemDiscount => UpdatePromotionUsage(currentUsages, lineItemDiscount));

                //create records for shipment discounts
                orderForm.Shipments.SelectMany(x => x.Discounts).ToList().ForEach(shipmentDiscount => UpdatePromotionUsage(currentUsages, shipmentDiscount));
            }

            MarketingRepository.UnitOfWork.Commit();
        }

        private PromotionUsage UpdatePromotionUsage(ICollection<PromotionUsage> currentUsages, Discount discount)
        {
            var usage = currentUsages.FirstOrDefault(x => x.PromotionId == discount.PromotionId);

            if (usage != null)
            {
                usage.Status = (int)UsageStatus;
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
                    Status = (int)UsageStatus,
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
