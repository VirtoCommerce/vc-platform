using ExpressionSerialization;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Actions;
using VirtoCommerce.MarketingModule.Web.Model.TypeExpressions.Conditions;

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

		}

		#endregion

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