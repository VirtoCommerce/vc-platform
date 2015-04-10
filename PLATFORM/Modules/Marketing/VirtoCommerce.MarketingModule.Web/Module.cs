using ExpressionSerialization;
using Microsoft.Practices.Unity;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Expressions.Promotion;
using VirtoCommerce.MarketingModule.Expressions;

namespace VirtoCommerce.MarketingModule.Web
{
    public class Module : IModule
    {
		private readonly IUnityContainer _container;

		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
			_container.RegisterType<IFoundationMarketingRepository>(new InjectionFactory(c => new FoundationMarketingRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor())));

			
			var promotionExtensionManager = new InMemoryPromotionExtensionManagerImpl();
			promotionExtensionManager.DynamicExpression = GetDynamicExpression();

			_container.RegisterInstance<IPromotionExtensionManager>(promotionExtensionManager);
			_container.RegisterType<IMarketingService, MarketingServiceImpl>();
			_container.RegisterType<IMarketingSearchService, MarketingSearchServiceImpl>();
			_container.RegisterType<IMarketingPromoEvaluator, DefaultMarketingPromoEvaluatorImpl>();

		}

		#endregion

		private static IDynamicExpression GetDynamicExpression()
		{
			var customerConditionBlock = new BlockCustomerCondition();
			customerConditionBlock.AvailableChildren = new DynamicExpression[] { new ConditionIsEveryone(), new ConditionIsFirstTimeBuyer(), 
																				  new ConditionIsRegisteredUser() }.ToList();

			var catalogConditionBlock = new BlockCatalogCondition();
			catalogConditionBlock.AvailableChildren = new DynamicExpression[] { new ConditionEntryIs(), new ConditionCurrencyIs(), 
																		       new  ConditionCodeContains(), new ConditionCategoryIs(), 
																			    }.ToList();

			var cartConditionBlock = new BlockCartCondition();
			cartConditionBlock.AvailableChildren = new DynamicExpression[] { new ConditionCartSubtotalLeast(), new ConditionAtNumItemsInCart(), 
																			 new ConditionAtNumItemsInCategoryAreInCart(), new ConditionAtNumItemsOfEntryAreInCart() }.ToList();
			var rewardBlock = new RewardBlock();
			rewardBlock.AvailableChildren = new DynamicExpression[] { new RewardCartGetOfAbsSubtotal(), new RewardItemGetFreeNumItemOfProduct(),  new RewardItemGetOfAbs(),
																	   new RewardItemGetOfAbsForNum(), new RewardItemGetOfRel(), new RewardItemGetOfRelForNum(),
																	   new RewardItemGiftNumItem(), new RewardShippingGetOfAbsShippingMethod(), new RewardShippingGetOfRelShippingMethod ()}.ToList();

			var rootBlocks = new DynamicExpression[] { customerConditionBlock, catalogConditionBlock, cartConditionBlock, rewardBlock }.ToList();
			var retVal = new PromoDynamicExpressionTree()
			{
				Children = rootBlocks,
				AvailableChildren = rootBlocks
			};
			return retVal;

		}
    }
}