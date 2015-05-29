using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.DynamicExpressionModule.Data;
using VirtoCommerce.DynamicExpressionModule.Data.Common;
using VirtoCommerce.DynamicExpressionModule.Data.Pricing;
using VirtoCommerce.DynamicExpressionModule.Data.Promotion;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.DynamicExpressionModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
        }

        public void Initialize()
        {
        }

        public void PostInitialize()
        {
			//Marketing expression
			var promotionExtensionManager = _container.Resolve<IMarketingExtensionManager>();

			promotionExtensionManager.PromotionDynamicExpressionTree = GetPromotionDynamicExpression();
			promotionExtensionManager.DynamicContentExpressionTree = GetContentDynamicExpression();

			//Pricing expression
			var pricingExtensionManager = _container.Resolve<IPricingExtensionManager>();
			pricingExtensionManager.ConditionExpressionTree = GetPricingDynamicExpression();
        }

        #endregion
		private static ConditionExpressionTree GetPricingDynamicExpression()
		{
			var conditions = new DynamicExpression[] { new ConditionGeoTimeZone(), new ConditionGeoZipCode(), new ConditionStoreSearchedPhrase() }.ToList();
			var rootBlock = new BlockPricingCondition { AvailableChildren = conditions };
			var retVal = new ConditionExpressionTree()
			{
				Children = new DynamicExpression[] { rootBlock }
			};
			return retVal;
		}

		private static ConditionExpressionTree GetContentDynamicExpression()
		{
			var conditions = new DynamicExpression[] { new ConditionGeoTimeZone(), new ConditionGeoZipCode(), new ConditionStoreSearchedPhrase() }.ToList();
			var rootBlock = new BlockContentCondition { AvailableChildren = conditions };
			var retVal = new ConditionExpressionTree()
			{
				Children = new DynamicExpression[] { rootBlock }
			};
			return retVal;
		}

		private static PromoDynamicExpressionTree GetPromotionDynamicExpression()
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
            };
            return retVal;
        }

    
    }
}
