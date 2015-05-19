using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Expressions;
using contentExpression = VirtoCommerce.MarketingModule.Expressions.Content;
using VirtoCommerce.MarketingModule.Expressions.Promotion;
using VirtoCommerce.MarketingModule.Web.Model;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

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

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
			using (var context = new MarketingRepositoryImpl())
			{
				var initializer = new SetupDatabaseInitializer<MarketingRepositoryImpl, VirtoCommerce.MarketingModule.Data.Migrations.Configuration>();
				initializer.InitializeDatabase(context);
			}
        }

        public void Initialize()
        {
			_container.RegisterType<IMarketingRepository>(new InjectionFactory(c => new MarketingRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));
          
            var promotionExtensionManager = new InMemoryExtensionManagerImpl();
            
            _container.RegisterInstance<IMarketingExtensionManager>(promotionExtensionManager);
            _container.RegisterType<IPromotionService, PromotionServiceImpl>();
			_container.RegisterType<IMarketingDynamicContentEvaluator, DefaultDynamicContentEvaluatorImpl>();
            _container.RegisterType<IDynamicContentService, DynamicContentServiceImpl>();
            _container.RegisterType<IMarketingSearchService, MarketingSearchServiceImpl>();
            _container.RegisterType<IMarketingPromoEvaluator, DefaultPromotionEvaluatorImpl>();
        }

        public void PostInitialize()
        {
			var promotionExtensionManager = _container.Resolve<IMarketingExtensionManager>();
			promotionExtensionManager.PromotionDynamicExpressionTree = GetPromotionDynamicExpression();
			promotionExtensionManager.DynamicContentExpressionTree = GetContentDynamicExpression();

            EnsureRootFoldersExist(new[] { MarketingConstants.ContentPlacesRootFolderId, MarketingConstants.CotentItemRootFolderId });
        }

        #endregion

		private static IDynamicExpression GetContentDynamicExpression()
		{
			var rootBlocks = new DynamicExpression[] { new contentExpression.ConditionGeoTimeZone(), new contentExpression.ConditionGeoZipCode(), new contentExpression.ConditionStoreSearchedPhrase() }.ToList();
			var retVal = new contentExpression.DynamicContentExpressionTree()
			{
				AvailableChildren = rootBlocks
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

        private void EnsureRootFoldersExist(string[] ids)
        {
            var dynamicContentService = _container.Resolve<IDynamicContentService>();
            foreach (var id in ids)
            {
                var rootFolder = dynamicContentService.GetFolderById(id);
                if (rootFolder == null)
                {
                    rootFolder = new Domain.Marketing.Model.DynamicContentFolder
                    {
                        Id = id,
                        Name = id
                    };
                    dynamicContentService.CreateFolder(rootFolder);
                }
            }

        }
    }
}
