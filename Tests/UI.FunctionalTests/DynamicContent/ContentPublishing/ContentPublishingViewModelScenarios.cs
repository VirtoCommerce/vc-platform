using CommerceFoundation.UI.FunctionalTests.TestHelpers;
using FunctionalTests.TestHelpers;
using System.Linq;
using UI.FunctionalTests.Helpers.Common;
using UI.FunctionalTests.Helpers.DynaminContent.ContentPublishing;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Implementations;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Implementations;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;
using Xunit;

namespace CommerceFoundation.UI.FunctionalTests.DynamicContent.ContentPublishing
{

    [Variant(RepositoryProvider.DataService)]
    public class ContentPublishingViewModelScenarios : FunctionalUITestBase
    {

        #region Infrastructure/ setup

        public override void DefService()
        {
            ServManager.AddService(ServiceNameEnum.DynamicContent);
        }

        #endregion


        [RepositoryTheory]
        public void Can_create_contentpublishingitemviewmodel_in_wizardmode()
        {
            var overviewVmFactory = new TestDynamicContentViewModelFactory<IContentPublishingOverviewStepViewModel>(ServManager.GetUri(ServiceNameEnum.DynamicContent));
            var placeVmFactory = new TestDynamicContentViewModelFactory<IContentPublishingContentPlacesStepViewModel>(ServManager.GetUri(ServiceNameEnum.DynamicContent));
            var contentVmFactory = new TestDynamicContentViewModelFactory<IContentPublishingDynamicContentStepViewModel>(ServManager.GetUri(ServiceNameEnum.DynamicContent));
            var conditionsVmFactory = new TestDynamicContentViewModelFactory<IContentPublishingConditionsStepViewModel>(ServManager.GetUri(ServiceNameEnum.DynamicContent));
            var repositoryFactory =
                new DSRepositoryFactory<IDynamicContentRepository, DSDynamicContentClient, DynamicContentEntityFactory>(ServManager.GetUri(ServiceNameEnum.DynamicContent));

            //creating additional objects
            DynamicContentPlace[] contentPlaces;
            DynamicContentItem[] contentItems;
            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                contentItems = TestContentItemsBuilder.BuildsContentItems().GetItems().ToArray();
                contentPlaces = TestContentPlacesBuilder.BuildContentPlaces().GetPlaces().ToArray();

                RepositoryHelper.AddItemToRepository(repository, contentItems.AsEnumerable());
                RepositoryHelper.AddItemToRepository(repository, contentPlaces.AsEnumerable());
            }

            var entityFactory = new DynamicContentEntityFactory();
            var item = entityFactory.CreateEntity<DynamicContentPublishingGroup>();

            //create viewmodel in wizardmode
            var createContentPublishngItemViewModel = new CreateContentPublishingItemViewModel(overviewVmFactory, placeVmFactory, contentVmFactory, conditionsVmFactory, item);
            Assert.NotNull(createContentPublishngItemViewModel);
            Assert.False(createContentPublishngItemViewModel.AllRegisteredSteps[0].IsValid);
            Assert.False(createContentPublishngItemViewModel.AllRegisteredSteps[1].IsValid);
            Assert.False(createContentPublishngItemViewModel.AllRegisteredSteps[2].IsValid);
            Assert.True(createContentPublishngItemViewModel.AllRegisteredSteps[3].IsValid);

            //fill the first step
            var firstStep = createContentPublishngItemViewModel.AllRegisteredSteps[0] as ContentPublishingOverviewStepViewModel;
            Assert.NotNull(firstStep);
            firstStep.InitializeForOpen();
            firstStep.InnerItem.Name = "NewTestName";
            firstStep.InnerItem.Description = "NewTestDescription";
            firstStep.InnerItem.Priority = 0;

            Assert.True(firstStep.IsValid);

            //fill the 2 step
            var secondStep =
                createContentPublishngItemViewModel.AllRegisteredSteps[1] as ContentPublishingContentPlacesStepViewModel;
            Assert.NotNull(secondStep);
            secondStep.InitializeForOpen();


            secondStep.InnerItemContentPlaces.Add(contentPlaces[0]);

            Assert.True(secondStep.IsValid);


            //fill the 3 step
            var thirdStep =
                createContentPublishngItemViewModel.AllRegisteredSteps[2] as
                    ContentPublishingDynamicContentStepViewModel;
            Assert.NotNull(thirdStep);
            thirdStep.InitializeForOpen();
            thirdStep.InnerItemDynamicContent.Add(contentItems[0]);

            Assert.True(thirdStep.IsValid);

            //fill the 4 step
            var fourthStep =
                createContentPublishngItemViewModel.AllRegisteredSteps[3] as ContentPublishingConditionsStepViewModel;
            Assert.NotNull(fourthStep);
            fourthStep.InitializeForOpen();

            var expression = TestContentPublishingExpressionBuilder.BuildContentPublishingExpressionBuilder(
                fourthStep.ExpressionElementBlock.Children[0])
                .AddCartTotalElement(fourthStep.ExpressionElementBlock.ExpressionViewModel)
                .AddConditionAddOrBlock(fourthStep.ExpressionElementBlock.ExpressionViewModel).GetChild();

            fourthStep.ExpressionElementBlock.Children[0] = expression;

            Assert.True(fourthStep.IsValid);

            createContentPublishngItemViewModel.PrepareAndSave();

            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                var itemFromDb =
                    repository.PublishingGroups.Where(
                        s => s.DynamicContentPublishingGroupId == firstStep.InnerItem.DynamicContentPublishingGroupId)
                        .Expand(cpg => cpg.ContentItems).Expand(cpg => cpg.ContentPlaces)
                        .SingleOrDefault();

                Assert.True(itemFromDb.Name == "NewTestName");
                Assert.True(itemFromDb.Description == "NewTestDescription");
                Assert.NotNull(itemFromDb.ConditionExpression);
                Assert.NotNull(itemFromDb.PredicateVisualTreeSerialized);
            }
        }

        [RepositoryTheory]
        public void Can_create_contentpublishingviewmodel_in_editmode_simpleacenarios()
        {
            var repositoryFactory =
                new DSRepositoryFactory<IDynamicContentRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                    ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var storeRepositoryFactory =
               new DSRepositoryFactory<IStoreRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                   ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var countryRepositoryFactory =
               new DSRepositoryFactory<ICountryRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                   ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var appConfigRepositoryFactory =
               new DSRepositoryFactory<IAppConfigRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                   ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var searchCategoryVmFactory =
               new TestDynamicContentViewModelFactory<ISearchCategoryViewModel>(
                   ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var navigationManager = new TestNavigationManager();

            DynamicContentPlace[] contentPlaces;
            DynamicContentItem[] contentItems;
            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                contentPlaces = TestContentPlacesBuilder.BuildContentPlaces().GetPlaces().ToArray();
                contentItems = TestContentItemsBuilder.BuildsContentItems().GetItems().ToArray();

                RepositoryHelper.AddItemToRepository(repository, contentPlaces.AsEnumerable());
                RepositoryHelper.AddItemToRepository(repository, contentItems.AsEnumerable());
            }

            //create fake innerItem
            var entityFactory = new DynamicContentEntityFactory();

            var item =
                TestContentPublishingBuilder.BuildDynamicContentPublishingGroup()
                    .WithContentItems(contentItems)
                    .WithContentPlaces(contentPlaces)
                    .GetContentPublishingGroup();

            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                RepositoryHelper.AddItemToRepository(repository, item);
            }

            var detailViewModel = new ContentPublishingItemViewModel(appConfigRepositoryFactory, countryRepositoryFactory, searchCategoryVmFactory, repositoryFactory, entityFactory, storeRepositoryFactory, navigationManager, item);
            Assert.NotNull(detailViewModel);
            detailViewModel.InitializeForOpen();



            //edit properties in detail viewmodel
            detailViewModel.InnerItem.Name = string.Empty;
            detailViewModel.InnerItem.Description = "EditDescription";
            detailViewModel.InnerItem.Priority = 23;

            detailViewModel.InnerItemDynamicContent.Clear();
            detailViewModel.InnerItemDynamicContent.Add(contentItems[1]);

            detailViewModel.InnerItemContentPlaces.Clear();
            detailViewModel.InnerItemContentPlaces.Add(contentPlaces[1]);

            detailViewModel.InnerItem.Name = "EditName";
            Assert.True(detailViewModel.IsValid);


            detailViewModel.SaveWithoutUIChanges();

            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                var itemFromDb =
                    repository.PublishingGroups.Where(
                        pg =>
                            pg.DynamicContentPublishingGroupId ==
                            detailViewModel.InnerItem.DynamicContentPublishingGroupId)
                        .Expand(pg => pg.ContentItems).Expand(pg => pg.ContentPlaces).SingleOrDefault();

                Assert.NotNull(itemFromDb);
                Assert.True(itemFromDb.ContentItems[0].DynamicContentItemId == contentItems[1].DynamicContentItemId);
                Assert.True(itemFromDb.ContentPlaces[0].DynamicContentPlaceId == contentPlaces[1].DynamicContentPlaceId);
                Assert.True(itemFromDb.Name == "EditName");
            }
        }

        [RepositoryTheory]
        public void Can_create_contentpublishingviewmodel_in_editmode_hardmode()
        {
            var repositoryFactory =
                new DSRepositoryFactory<IDynamicContentRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                    ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var storeRepositoryFactory =
                new DSRepositoryFactory<IStoreRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                    ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var countryRepositoryFactory =
                new DSRepositoryFactory<ICountryRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                    ServManager.GetUri(ServiceNameEnum.DynamicContent));
            var appConfigRepositoryFactory =
               new DSRepositoryFactory<IAppConfigRepository, DSDynamicContentClient, DynamicContentEntityFactory>(
                   ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var searchCategoryVmFactory =
               new TestDynamicContentViewModelFactory<ISearchCategoryViewModel>(
                   ServManager.GetUri(ServiceNameEnum.DynamicContent));

            var navigationManager = new TestNavigationManager();

            DynamicContentPlace[] contentPlaces;
            DynamicContentItem[] contentItems;
            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                contentPlaces = TestContentPlacesBuilder.BuildContentPlaces().GetPlaces().ToArray();
                contentItems = TestContentItemsBuilder.BuildsContentItems().GetItems().ToArray();

                RepositoryHelper.AddItemToRepository(repository, contentItems.AsEnumerable());
                RepositoryHelper.AddItemToRepository(repository, contentPlaces.AsEnumerable());
            }

            //create fake innerItem
            var entityFactory = new DynamicContentEntityFactory();

            var item =
                TestContentPublishingBuilder.BuildDynamicContentPublishingGroup()
                    .WithContentItems(contentItems.Take(1).ToArray())
                    .WithContentPlaces(contentPlaces.Take(1).ToArray())
                    .GetContentPublishingGroup();

            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                RepositoryHelper.AddItemToRepository(repository, item);
            }

            var detailViewModel = new ContentPublishingItemViewModel(appConfigRepositoryFactory, countryRepositoryFactory, searchCategoryVmFactory, repositoryFactory, entityFactory, storeRepositoryFactory, navigationManager, item);
            Assert.NotNull(detailViewModel);
            detailViewModel.InitializeForOpen();

            detailViewModel.InnerItem.Name = "EditName";

            //edit 2 step
            detailViewModel.InnerItemContentPlaces.Add(contentPlaces[1]);
            detailViewModel.InnerItemContentPlaces.Remove(contentPlaces[1]);
            detailViewModel.InnerItemContentPlaces.Add(contentPlaces[1]);
            detailViewModel.InnerItemContentPlaces.Remove(contentPlaces[1]);
            detailViewModel.InnerItemContentPlaces.Add(contentPlaces[1]);

            //edit 3 step
            detailViewModel.InnerItemDynamicContent.Add(contentItems[2]);
            detailViewModel.InnerItemDynamicContent.Remove(contentItems[2]);
            detailViewModel.InnerItemDynamicContent.Add(contentItems[2]);
            detailViewModel.InnerItemDynamicContent.Remove(contentItems[2]);


            //edit 4 step
            var expressionViewModel = detailViewModel.ExpressionElementBlock.ExpressionViewModel;
            detailViewModel.ExpressionElementBlock.Children[0] =
                TestContentPublishingExpressionBuilder.BuildContentPublishingExpressionBuilder(
                    detailViewModel.ExpressionElementBlock.Children[0])
                    .AddCartTotalElement(expressionViewModel).AddConditionAddOrBlock(expressionViewModel).GetChild();

            Assert.True(detailViewModel.IsValid);
            detailViewModel.SaveWithoutUIChanges();


            //check the item from db
            using (var repository = repositoryFactory.GetRepositoryInstance())
            {
                var itemFromDb =
                    repository.PublishingGroups.Where(
                        pg =>
                            pg.DynamicContentPublishingGroupId ==
                            detailViewModel.InnerItem.DynamicContentPublishingGroupId)
                        .Expand(pg => pg.ContentItems).Expand(pg => pg.ContentPlaces).SingleOrDefault();

                Assert.NotNull(itemFromDb);
                Assert.True(itemFromDb.ContentItems.Count == 1);
                Assert.True(itemFromDb.ContentPlaces.Count == 2);
                Assert.True(itemFromDb.Name == "EditName");
                Assert.NotNull(itemFromDb.ConditionExpression);
                Assert.NotNull(itemFromDb.PredicateVisualTreeSerialized);
            }


        }

    }
}
