using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using CacheManager.Core;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.DynamicExpressionModule.Data.Promotion;
using VirtoCommerce.MarketingModule.Data.Migrations;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Test.CustomDynamicPromotionExpressions;
using VirtoCommerce.MarketingModule.Web.Controllers.Api;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Tests.Bases;
using Xunit;
using dataModel = VirtoCommerce.MarketingModule.Data.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Test
{
    public class MarketingControllerScenarios : FunctionalTestBase
    {
        [Fact]
        [Trait("Category", "CI")]
        public void Can_create_marketing_contentitem()
        {
            var repository = this.GetRepository();
            var contentItem = new dataModel.DynamicContentItem()
            {
                Name = "ss",
            };

            repository.Add(contentItem);
            repository.UnitOfWork.Commit();
        }

        [Fact]
        [Trait("Category", "CI")]
        public void Can_create_marketing_dynamicpromotion_using_api()
        {
            var marketingController = GetMarketingController(GetPromotionExtensionManager());

            webModel.Promotion promotion = null;
            var promoResult = (marketingController.GetPromotionById("CartFiveDiscount") as OkNegotiatedContentResult<webModel.Promotion>);
            if (promoResult != null)
            {
                promotion = promoResult.Content;
            }
            if (promotion == null)
            {
                promotion = (marketingController.GetNewDynamicPromotion() as OkNegotiatedContentResult<webModel.Promotion>).Content;

                promotion.Description = "Buy at $100 and get a 5% discount";
                promotion.Name = "CartFiveDiscount";
                promotion.Id = "CartFiveDiscount";

                var expressionTree = promotion.DynamicExpression as DynamicExpression;

                //Curreny is USD
                var currencyExpression = expressionTree.FindAvailableExpression<ConditionCurrencyIs>();
                currencyExpression.Currency = "USD";
                expressionTree.Children.Add(currencyExpression);
                //Condition: Cart subtotal great or equal that 100$
                var subtotalExpression = expressionTree.FindAvailableExpression<ConditionCartSubtotalLeast>();
                subtotalExpression.SubTotal = 100;
                expressionTree.Children.Add(subtotalExpression);
                //Reward: Get 5% whole cart discount
                var rewardExpr = expressionTree.FindAvailableExpression<RewardItemGetOfRel>();
                rewardExpr.Amount = 0.5m;
                expressionTree.Children.Add(rewardExpr);

                promotion = (marketingController.CreatePromotion(promotion) as OkNegotiatedContentResult<webModel.Promotion>).Content;
            }

            var cacheManager = new Moq.Mock<ICacheManager<object>>();
            var marketingEval = new DefaultPromotionEvaluatorImpl(GetMarketingService(), cacheManager.Object);
            var context = GetPromotionEvaluationContext();
            var result = marketingEval.EvaluatePromotion(context);
        }

        [Fact]
        [Trait("Category", "CI")]
        public void Can_extend_marketing_promotion_expressiontree_and_create_new_dynamicpromotion()
        {
            var extensionManager = GetPromotionExtensionManager();

            //Register custom dynamic expression in main expression tree now it should be availabe for ui in expression builder
            var blockExpression = extensionManager.PromotionDynamicExpressionTree as DynamicExpression;
            var blockCatalogCondition = blockExpression.FindChildrenExpression<BlockCatalogCondition>();
            blockCatalogCondition.AvailableChildren.Add(new ConditionItemWithTag());

            var marketingController = GetMarketingController(extensionManager);

            //Create custom promotion
            webModel.Promotion promotion = null;
            var promoResult = (marketingController.GetPromotionById("TaggedProductDiscount") as OkNegotiatedContentResult<webModel.Promotion>);
            if (promoResult != null)
            {
                promotion = promoResult.Content;
            }
            if (promotion == null)
            {
                promotion = (marketingController.GetNewDynamicPromotion() as OkNegotiatedContentResult<webModel.Promotion>).Content;

                promotion.Description = "Buy all product with tag '#FOOTBAL' with 7% discount";
                promotion.Name = "TaggedProductDiscount";
                promotion.Id = "TaggedProductDiscount";

                blockExpression = promotion.DynamicExpression as DynamicExpression;
                blockCatalogCondition = blockExpression.FindChildrenExpression<BlockCatalogCondition>();
                var blockReward = blockExpression.FindChildrenExpression<RewardBlock>();

                var conditionExpr = blockCatalogCondition.FindAvailableExpression<ConditionItemWithTag>();
                conditionExpr.Tags = new string[] { "#FOOTBAL" };
                blockCatalogCondition.Children.Add(conditionExpr);

                var rewardExpr = blockReward.FindAvailableExpression<RewardItemGetOfRel>();
                rewardExpr.Amount = 0.7m;
                blockReward.Children.Add(rewardExpr);

                promotion = (marketingController.CreatePromotion(promotion) as OkNegotiatedContentResult<webModel.Promotion>).Content;
            }

            var cacheManager = new Moq.Mock<ICacheManager<object>>();
            var marketingEval = new DefaultPromotionEvaluatorImpl(GetMarketingService(), cacheManager.Object);
            var context = GetPromotionEvaluationContext();
            context.PromoEntries.First().Attributes["tag"] = "#FOOTBAL";
            var result = marketingEval.EvaluatePromotion(context);
        }


        [Fact]
        public void EvaluatePromotionWhenCatalogBrowsing()
        {
        }

        [Fact]
        public void EvaluateCartPromotion()
        {
        }

        private static PromotionEvaluationContext GetPromotionEvaluationContext()
        {
            var context = new PromotionEvaluationContext();
            context.CartPromoEntries = context.PromoEntries = new[]
            {
                    new ProductPromoEntry
                    {
                        CatalogId = "Samsung",
                        CategoryId = "100df6d5-8210-4b72-b00a-5003f9dcb79d",
                        ProductId = "v-b000bkzs9w",
                        Price = 160.44m,
                        Quantity = 2,

                    },
                    new ProductPromoEntry
                    {
                        CatalogId = "Sony",
                        CategoryId = "100df6d5-8210-4b72-b00a-5003f9dcb79d",
                        ProductId = "v-a00032ksj",
                        Price = 12.00m,
                        Quantity = 1,

                    },
                    new ProductPromoEntry
                    {
                        CatalogId = "LG",
                        CategoryId = "100df6d5-8210-4b72-b00a-5003f9dcb79d",
                        ProductId = "v-c00021211",
                        Price = 1.00m,
                        Quantity = 1,

                    }
            };
            return context;
        }

        private IPromotionService GetMarketingService()
        {
            Func<IMarketingRepository> foundationRepositoryFactory = () => GetRepository();
            var promotionExtensionManager = new DefaultMarketingExtensionManagerImpl();
            var cacheManager = new Moq.Mock<ICacheManager<object>>();
            var retVal = new PromotionServiceImpl(foundationRepositoryFactory, promotionExtensionManager, cacheManager.Object);
            return retVal;
        }

        private MarketingModulePromotionController GetMarketingController(IMarketingExtensionManager extensionManager)
        {
            var retVal = new MarketingModulePromotionController(GetMarketingService(), extensionManager, null);
            return retVal;
        }

        private IMarketingExtensionManager GetPromotionExtensionManager()
        {
            var retVal = new DefaultMarketingExtensionManagerImpl();
            retVal.PromotionDynamicExpressionTree = GetDynamicExpression();
            return retVal;
        }

        private PromoDynamicExpressionTree GetDynamicExpression()
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
            rewardBlock.AvailableChildren = new DynamicExpression[] { new RewardCartGetOfAbsSubtotal(),  new RewardItemGetFreeNumItemOfProduct(),  new RewardItemGetOfAbs(),
                                                                       new RewardItemGetOfAbsForNum(), new RewardItemGetOfRel(), new RewardItemGetOfRelForNum(),
                                                                       new RewardItemGiftNumItem(), new RewardShippingGetOfAbsShippingMethod(), new RewardShippingGetOfRelShippingMethod ()}.ToList();

            var rootBlockExpressions = new DynamicExpression[] { customerConditionBlock, catalogConditionBlock, cartConditionBlock, rewardBlock }.ToList();
            var retVal = new PromoDynamicExpressionTree()
            {
                Children = rootBlockExpressions,
                AvailableChildren = rootBlockExpressions
            };
            return retVal;

        }

        protected IMarketingRepository GetRepository()
        {
            var repository = new MarketingRepositoryImpl(ConnectionString, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            EnsureDatabaseInitialized(() => new MarketingRepositoryImpl(ConnectionString), () => Database.SetInitializer(new SetupDatabaseInitializer<MarketingRepositoryImpl, Configuration>()));
            return repository;
        }

        public override void Dispose()
        {
            try
            {
                // Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
                // the temp location in which they are stored is later cleaned.
                using (var context = new MarketingRepositoryImpl(ConnectionString))
                {
                    context.Database.Delete();
                }
            }
            finally
            {
            }
        }
    }
}
