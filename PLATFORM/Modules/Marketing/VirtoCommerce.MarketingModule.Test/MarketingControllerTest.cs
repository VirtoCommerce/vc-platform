using System;
using System.Linq;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.MarketingModule.Data;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Web.Controllers.Api;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;
using Xunit;
using System.Web.Http.Results;

namespace VirtoCommerce.MarketingModule.Test
{
	public class MarketingControllerTest
	{
		[Fact]
		public void CreateDynamicPromotion()
		{
			var marketingController = GetMarketingController();

			var promotion = (marketingController.GetPromotionById("CartFiveDiscount") as OkNegotiatedContentResult<webModel.Promotion>).Content;
			if (promotion == null)
			{
				promotion = (marketingController.GetNewDynamicPromotion() as OkNegotiatedContentResult<webModel.Promotion>).Content;

				promotion.Description = "Buy at $100 and get a 5% discount";
				promotion.Name = "CartFiveDiscount";
				var cartConditionBlock = promotion.DynamicExpression.AvailableChildren.OfType<BlockCartCondition>().First();

				var condition = cartConditionBlock.AvailableChildren.OfType<ConditionCartSubtotalLeast>().First();
				condition.SubTotal = 100;
				cartConditionBlock.Children = new IDynamicExpression[] { condition };


				var rewardBlock = promotion.DynamicExpression.AvailableChildren.OfType<RewardBlock>().First();
				var subtotalReward = rewardBlock.AvailableChildren.OfType<RewardCartGetOfRelSubtotal>().First();
				subtotalReward.Amount = 0.5m;
				rewardBlock.Children = new IDynamicExpression[] { subtotalReward };

				promotion.DynamicExpression.Children = new IDynamicExpression[] { cartConditionBlock, rewardBlock };
				promotion = (marketingController.CreatePromotion(promotion) as OkNegotiatedContentResult<webModel.Promotion>).Content;

			}

			var context = new PromotionEvaluationContext();
			context.ShoppingCart = new ShoppingCart
			{
				Items = new LineItem[] {  
					new LineItem
					{
						Id = "1",
						CatalogId = "Samsung",
						CategoryId = "100df6d5-8210-4b72-b00a-5003f9dcb79d",
						ProductId = "v-b000bkzs9w",
						ListPrice = 160.44m,
						PlacedPrice = 206.33m,
						Quantity = 5,
						Name = "Samsung YP-T7JX 512 MB Digital Audio Player with FM Tuner & Recorder",
					}
				}
			};
			var marketingEval = new DefaultMarketingPromoEvaluatorImpl(GetMarketingService());
			var result = marketingEval.EvaluatePromotion(context);
		
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


			var promotionExtensionManager = new InMemoryPromotionExtensionManagerImpl();
			promotionExtensionManager.DynamicExpression = GetDynamicExpression();
			var retVal = new MarketingManagmentController(new MarketingServiceImpl(foundationRepositoryFactory, promotionExtensionManager), null, promotionExtensionManager);
			return retVal;
		}

		private static PromoDynamicPromotionExpression GetDynamicExpression()
		{
			var customerConditionBlock = new BlockCustomerCondition();
			customerConditionBlock.AvailableChildren = new IDynamicExpression[] { new ConditionIsEveryone(), new ConditionIsFirstTimeBuyer(), 
																				  new ConditionIsRegisteredUser() };

			var catalogConditionBlock = new BlockCatalogCondition();
			catalogConditionBlock.AvailableChildren = new IDynamicExpression[] { new ConditionEntryIs(), new ConditionCurrencyIs(), 
																		       new  ConditionCodeContains(), new ConditionCategoryIs(), 
																			    };

			var cartConditionBlock = new BlockCartCondition();
			cartConditionBlock.AvailableChildren = new IDynamicExpression[] { new ConditionCartSubtotalLeast(), new ConditionAtNumItemsInCart(), 
																			 new ConditionAtNumItemsInCategoryAreInCart(), new ConditionAtNumItemsOfEntryAreInCart() };
			var rewardBlock = new RewardBlock();
			rewardBlock.AvailableChildren = new IDynamicExpression[] { new RewardCartGetOfAbsSubtotal(), new RewardCartGetOfRelSubtotal(), 
																	   new RewardItemGetFreeNumItemOfProduct(),  new RewardItemGetOfAbs(),
																	   new RewardItemGetOfAbsForNum(), new RewardItemGetOfRel(), new RewardItemGetOfRelForNum(),
																	   new RewardItemGiftNumItem(), new RewardShippingGetOfAbsShippingMethod(), new RewardShippingGetOfRelShippingMethod ()};


			var retVal = new PromoDynamicPromotionExpression()
			{
				AvailableChildren = new IDynamicExpression[] { customerConditionBlock, catalogConditionBlock, cartConditionBlock, rewardBlock }
			};
			return retVal;

		}
	}
}
