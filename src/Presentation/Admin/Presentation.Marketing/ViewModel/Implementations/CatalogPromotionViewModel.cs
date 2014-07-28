using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Marketing.Model;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Implementations
{
    public class CatalogPromotionViewModel : PromotionViewModelBase, ICatalogPromotionViewModel
    {
        #region Dependencies

        private readonly IRepositoryFactory<ICatalogRepository> _catalogRepositoryFactory;
        private readonly IRepositoryFactory<IPricelistRepository> _pricelistRepositoryFactory;

        private const int TabIndexOverview = 0;
        private const int TabIndexConditions = 1;
        #endregion

        #region Constructor

        public CatalogPromotionViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IRepositoryFactory<IMarketingRepository> repositoryFactory,
            IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
            IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
            IViewModelsFactory<ISearchItemViewModel> searchItemVmFactory,
            IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
            IMarketingEntityFactory entityFactory,
            INavigationManager navManager,
            Promotion item)
            : base(appConfigRepositoryFactory, repositoryFactory, entityFactory, navManager, item, false, searchCategoryVmFactory, searchItemVmFactory, shippingRepositoryFactory)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _pricelistRepositoryFactory = pricelistRepositoryFactory;

            ViewTitle = new ViewTitleBase
                {
                    Title = "Promotion",
                    SubTitle =
                        (item != null && !string.IsNullOrEmpty(item.Name))
                            ? item.Name.ToUpper(CultureInfo.InvariantCulture)
                            : string.Empty
                };
        }

        public bool IsWizard
        {
            get { return !IsSingleDialogEditing; }
        }


        protected CatalogPromotionViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IRepositoryFactory<IMarketingRepository> repositoryFactory,
            IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
            IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
            IViewModelsFactory<ISearchItemViewModel> searchItemVmFactory,
            IRepositoryFactory<IShippingRepository> shippingRepositoryFactory,
            IMarketingEntityFactory entityFactory,
            Promotion item)
            : base(appConfigRepositoryFactory, repositoryFactory, entityFactory, null, item, true, searchCategoryVmFactory, searchItemVmFactory, shippingRepositoryFactory)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _pricelistRepositoryFactory = pricelistRepositoryFactory;
            IsWizardMode = true;
        }

        #endregion

        #region ICatalogPromotionViewModel Members

        public List<catalogModel.CatalogBase> AvailableCatalogs { get; private set; }

        public catalogModel.CatalogBase InnerCatalog { get; set; }

        #endregion

        #region Override PromotionViewModelBase Methods

        protected override void InitializePropertiesForViewing()
        {
            if (!IsWizardMode)
            {
                base.InitializePropertiesForViewing();
                InitializeAvailableCatalogs();
            }
        }

        public override string CatalogId
        {
            get
            {
                return (InnerItem as CatalogPromotion).CatalogId;
            }
        }

        protected override void InitializeExpressionElementBlock()
        {
            if (IsWizardMode)
            {
                OnUIThread(() =>
                {
                    ExpressionElementBlock = new CatalogPromotionExpressionBlock(this);
                    InnerItem.PredicateVisualTreeSerialized = SerializationUtil.Serialize(ExpressionElementBlock);
                });
            }
            base.InitializeExpressionElementBlock();
        }

        #endregion

        #region  Initialize and Update

        protected void InitializeAvailableCatalogs()
        {
            if (AvailableCatalogs == null)
            {
                using (var repository = _catalogRepositoryFactory.GetRepositoryInstance())
                {
                    var pricelistAssignements = _pricelistRepositoryFactory.GetRepositoryInstance().PricelistAssignments.ToList();

                    var catalogs = repository.Catalogs.OrderBy(catalog => catalog.Name).ToList();

                    var items = catalogs.Where(cat => pricelistAssignements.Any(assignment => assignment.CatalogId == cat.CatalogId)).ToList();
                    items.Insert(0, new catalogModel.Catalog() { CatalogId = null, Name = "Select catalog...".Localize() });
                    OnUIThread(() =>
                    {
                        AvailableCatalogs = items;
                        OnPropertyChanged("AvailableCatalogs");
                    });
                }
            }
        }

        protected override bool IsValidForSave()
        {
            var result = base.IsValidForSave();
            var isExpressionValid = IsWizardMode || (ExpressionElementBlock is CatalogPromotionExpressionBlock &&
                                                    (ExpressionElementBlock as CatalogPromotionExpressionBlock).GetPromotionRewards().Any() &&
                                                    (ExpressionElementBlock as CatalogPromotionExpressionBlock).ConditionBlock.Children.Any());

            if (!result)
                SelectedTabIndex = TabIndexOverview;
            else if (!isExpressionValid)
                SelectedTabIndex = TabIndexConditions;

            return result && isExpressionValid;
        }

        #endregion

    }
}
