using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Web.Controllers.Api;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    public class MarketingControllerTest
    {
		[Fact]
		public void EvaluatePromotion()
		{
			var marketingService = GetMarketingService();
			var promotion = marketingService.GetPromotionById("cf0db89d-cc8f-4444-b17c-56cb05cfff10");
			var context = new PromotionEvaluationContext();
			context.ShoppingCart = new ShoppingCart
			{
				Items = new LineItem[] {  
					new LineItem
					{
						CatalogId = "Samsung",
						CategoryId = "100df6d5-8210-4b72-b00a-5003f9dcb79d",
						ProductId = "v-b000bkzs9w",
						ListPrice = 10.44m,
						PlacedPrice = 20.33m,
						Quantity = 5,
						Name = "Samsung YP-T7JX 512 MB Digital Audio Player with FM Tuner & Recorder",
					}
				}
			};
			var result = promotion.EvaluatePromotion(context);
		}

		private static IMarketingService GetMarketingService()
		{
			Func<IFoundationMarketingRepository> foundationRepositoryFactory = () => new FoundationMarketingRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			var promotionExtensionManager = new InMemoryPromotionExtensionManagerImpl();
			var retVal = new MarketingServiceImpl(foundationRepositoryFactory, promotionExtensionManager);
			return retVal;
		}

		private static MarketingManagmentController GetMarketingController()
		{
			Func<IFoundationMarketingRepository> foundationRepositoryFactory = () => new FoundationMarketingRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());

			var customerConditionBlock = new BlockCustomerCondition();
			var catalogConditionBlock = new BlockCatalogCondition();
			var cartConditionBlock = new BlockCartCondition();
			var rewardBlock = new RewardBlock();

			cartConditionBlock.AvailableChildren = new IDynamicExpression[] { new ConditionAtNumItemsInCart() };
			rewardBlock.AvailableChildren = new IDynamicExpression[] { new RewardCartGetOfAbsSubtotal() };

			var promotionDynamicExpr = new DynamicPromotionExpression()
			{

				AvailableChildren = new IDynamicExpression[] { customerConditionBlock, catalogConditionBlock, cartConditionBlock, rewardBlock }
			};
			var promotionExtensionManager = new InMemoryPromotionExtensionManagerImpl();
			promotionExtensionManager.DynamicExpression = promotionDynamicExpr;
			var retVal = new MarketingManagmentController(new MarketingServiceImpl(foundationRepositoryFactory, promotionExtensionManager), null, promotionExtensionManager);
			return retVal;
		}
    }
}
