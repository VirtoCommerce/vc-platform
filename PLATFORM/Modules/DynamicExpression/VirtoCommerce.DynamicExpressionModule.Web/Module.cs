using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.DynamicExpressionModule.Data;
using VirtoCommerce.DynamicExpressionModule.Data.Content;
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
			var promotionExtensionManager = _container.Resolve<IMarketingExtensionManager>();
			promotionExtensionManager.PromotionDynamicExpressionTree = GetPromotionDynamicExpression();
			promotionExtensionManager.DynamicContentExpressionTree = GetContentDynamicExpression();
        }

        #endregion

		private static IDynamicExpression GetContentDynamicExpression()
		{
			var conditions = new DynamicExpression[] { new ConditionGeoTimeZone(), new ConditionGeoZipCode(), new ConditionStoreSearchedPhrase() }.ToList();
			var rootBlock = new BlockContentCondition { AvailableChildren = conditions };
			var retVal = new DynamicContentExpressionTree()
			{
				Children = new DynamicExpression[] { rootBlock }
			};
			return retVal;
		}

        private static IDynamicExpression GetPromotionDynamicExpression()
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
