using System;
using System.Linq;
using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using FunctionalTests.TestHelpers;
using UI.FunctionalTests.Helpers.Common;
using UI.FunctionalTests.Helpers.Marketing;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.ManagementClient.Marketing.Model;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Wizard.Interfaces;
using Xunit;

namespace CommerceFoundation.UI.FunctionalTests.DynamicContent.ContentPublishing
{

    [Variant(RepositoryProvider.DataService)]
	public class CartPromotionViewModelScenarios : FunctionalUITestBase
    {

        #region Infrastructure/ setup

		public override void DefService()
		{
			ServManager.AddService(ServiceNameEnum.Marketing);
			ServManager.AddService(ServiceNameEnum.Order);
			ServManager.AddService(ServiceNameEnum.Catalog);
			ServManager.AddService(ServiceNameEnum.AppConfig);
			ServManager.AddService(ServiceNameEnum.Store);
		}

	    #endregion

    
        [RepositoryTheory]
        public void Can_create_cartpromotionviewmodel_in_wizardmode()
        {
			var overviewVmFactory = new TestMarketingViewModelFactory<ICartPromotionOverviewStepViewModel>(ServManager.GetUri(ServiceNameEnum.Marketing), ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));
			var couponsVmFactory = new TestMarketingViewModelFactory<ICartPromotionCouponStepViewModel>(ServManager.GetUri(ServiceNameEnum.Marketing), ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));
			var conditionsVmFactory = new TestMarketingViewModelFactory<ICartPromotionExpressionStepViewModel>(ServManager.GetUri(ServiceNameEnum.Marketing), ServManager.GetUri(ServiceNameEnum.Catalog), ServManager.GetUri(ServiceNameEnum.Store), ServManager.GetUri(ServiceNameEnum.Order), ServManager.GetUri(ServiceNameEnum.AppConfig));

            var repositoryFactory =
                new DSRepositoryFactory<IMarketingRepository, DSMarketingClient, MarketingEntityFactory>(
					ServManager.GetUri(ServiceNameEnum.Marketing));

            
            var entityFactory = new MarketingEntityFactory();
            var item = entityFactory.CreateEntity<CartPromotion>();

            //create viewmodel in wizardmode
            var createCartPromotionViewModel = new CreateCartPromotionViewModel(overviewVmFactory, conditionsVmFactory, couponsVmFactory, item);
			Assert.NotNull(createCartPromotionViewModel);
			Assert.False(createCartPromotionViewModel.AllRegisteredSteps[0].IsValid);
			Assert.False(createCartPromotionViewModel.AllRegisteredSteps[1].IsValid);
			Assert.True(createCartPromotionViewModel.AllRegisteredSteps[2].IsValid);
			
            //fill the first step
            var firstStep = createCartPromotionViewModel.AllRegisteredSteps[0] as CartPromotionOverviewStepViewModel;
            Assert.NotNull(firstStep);
            firstStep.InitializeForOpen();
	        (firstStep.InnerItem as CartPromotion).StoreId = "TestStore";
	        firstStep.InnerItem.StartDate = DateTime.UtcNow;
            firstStep.InnerItem.Name = "NewTestName";
            firstStep.InnerItem.Description = "NewTestDescription";
            firstStep.InnerItem.Priority = 0;

            Assert.True(firstStep.IsValid);
			
            //fill the 2 step (expression builder)
			var secondStep =
				createCartPromotionViewModel.AllRegisteredSteps[1] as CartPromotionExpressionStepViewModel;
            Assert.NotNull(secondStep);
			secondStep.InitializeForOpen();
	        var expression = CartPromotionExpressionBuilderHelper.BuildCartPromotionExpressionBuilder(
		        (CartPromotionExpressionBlock) secondStep.ExpressionElementBlock)
	                                                             .AddEveryoneEligibility(secondStep)
																 .AddNumItemsInCartElement(secondStep, false).GetChild();
				//.AddConditionAddOrBlock(secondStep).GetChild();

			secondStep.ExpressionElementBlock.Children[0] = expression;

			Assert.False(secondStep.IsValid);
			
			//fill the 3 step
			var thirdStep =
				createCartPromotionViewModel.AllRegisteredSteps[2] as
					CartPromotionCouponStepViewModel;
			Assert.NotNull(thirdStep);
			thirdStep.InitializeForOpen();
			thirdStep.HasCoupon = true;
			Assert.False(createCartPromotionViewModel.AllRegisteredSteps[2].IsValid);
	        thirdStep.InnerItem.Coupon = new Coupon() {Code = "testCoupon"};
			Assert.True(createCartPromotionViewModel.AllRegisteredSteps[2].IsValid);

			Assert.True(thirdStep.IsValid);

			createCartPromotionViewModel.PrepareAndSave();

            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                var itemFromDb =
                    repository.Promotions.Where(
                        s => s.PromotionId == firstStep.InnerItem.PromotionId)
                        .Expand(cpg=>cpg.Rewards)
                        .SingleOrDefault();

                Assert.True(itemFromDb.Name == "NewTestName");
                Assert.True(itemFromDb.Description == "NewTestDescription");
                Assert.NotNull(itemFromDb.PredicateSerialized);
                Assert.NotNull(itemFromDb.PredicateVisualTreeSerialized);
            }
        }
    }
}
