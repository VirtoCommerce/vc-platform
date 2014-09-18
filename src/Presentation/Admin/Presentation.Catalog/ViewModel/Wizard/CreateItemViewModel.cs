using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
    public class CreateItemViewModel : WizardContainerStepsViewModel, ICreateItemViewModel
    {
        private readonly IItemPricingStepViewModel pricingStep4;
        readonly ItemStepModel _itemModel;

        public CreateItemViewModel(
            IViewModelsFactory<IItemOverviewStepViewModel> overviewVmFactory,
            IViewModelsFactory<IItemPropertiesStepViewModel> propertiesVmFactory,
            IViewModelsFactory<IItemPricingStepViewModel> pricingVmFactory,
            IViewModelsFactory<IEditorialReviewViewModel> reviewVmFactory,
            Item item, IViewModel parentEntityVM, ICatalogEntityFactory entityFactory)
        {
            _itemModel = new ItemStepModel
                {
                    InnerItem = item,
                    ParentEntityVM = parentEntityVM,
                    ParentWizard = this
                };

            var allParameters = new[] { new KeyValuePair<string, object>("itemModel", _itemModel) };

            // properties Step must be created first
            var propertiesStep = propertiesVmFactory.GetViewModelInstance(allParameters);
            // this step is created second, but registered first
            RegisterStep(overviewVmFactory.GetViewModelInstance(allParameters));
            RegisterEditorialReviewStep(item, entityFactory, reviewVmFactory);

            // properties Step is registered third
            RegisterStep(propertiesStep);

            pricingStep4 = pricingVmFactory.GetViewModelInstance(allParameters);
            // this step is added or removed at RUNTIME
            // RegisterStep(pricingStep4);

            item.StartDate = DateTime.Today;
        }

        public void RegisterPricingStep(bool register)
        {
            if (register)
            {
                if (!AllRegisteredSteps.Contains(pricingStep4))
                {
                    RegisterStep(pricingStep4);
                    if (pricingStep4.IsInitializingPricing)
                    {
                        var curStep = pricingStep4 as ISupportDelayInitialization;
                        Task.Run(() => curStep.InitializeForOpen());
                    }
                }
            }
            else if (AllRegisteredSteps.Contains(pricingStep4))
            {
                UnregisterStep(pricingStep4);
            }
        }

        private void RegisterEditorialReviewStep(Item parentItem, ICatalogEntityFactory entityFactory, IViewModelsFactory<IEditorialReviewViewModel> vmFactory)
        {
            var item = (EditorialReview)entityFactory.CreateEntityForType(typeof(EditorialReview));
            item.CatalogItem = parentItem;
            item.Locale = parentItem.Catalog.DefaultLanguage;
            item.Priority = 1;
            item.Source = "Product Description".Localize();

            var retVal = vmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("isWizardMode", true));
            RegisterStep(retVal);
        }
    }

    public class ItemStepModel
    {
        public Item InnerItem;
        public ObservableCollection<PropertyAndPropertyValueBase> PropertiesAndValues;
        public List<string> InnerItemCatalogLanguages;
        public CreateItemViewModel ParentWizard;
        public IViewModel ParentEntityVM;
        public IPricelistRepository PricelistRepository;
    }

    public abstract class ItemStepViewModel : ItemViewModel
    {
        protected ItemStepModel stepModel;

        protected ItemStepViewModel(
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory,
            IViewModelsFactory<IPropertyValueBaseViewModel> vmFactory,
            ICatalogEntityFactory entityFactory,
            IAuthenticationContext authContext,
            ItemStepModel itemModel,
            IViewModelsFactory<IPriceViewModel> priceVmFactory)
            : base(repositoryFactory, pricelistRepositoryFactory, vmFactory, entityFactory, itemModel.InnerItem, authContext, priceVmFactory)
        {
            stepModel = itemModel;
        }

        public override bool IsLast
        {
            get
            {
                // if (!(InnerItem is Product))
                if (InnerItem.IsBuyable)
                    return this is IItemPricingStepViewModel;
                else
                    return this is IItemPropertiesStepViewModel;
            }
        }

        protected override void DoSaveChanges()
        {
            _priceListRepository = stepModel.PricelistRepository;

            base.DoSaveChanges();
        }
    }

    public class ItemOverviewStepViewModel : ItemStepViewModel, IItemOverviewStepViewModel, ISupportWizardPrepare
    {
        public ItemOverviewStepViewModel(
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IViewModelsFactory<IPropertyValueBaseViewModel> vmFactory,
            ICatalogEntityFactory entityFactory,
            IAuthenticationContext authContext,
            ItemStepModel itemModel)
            : base(repositoryFactory, null, vmFactory, entityFactory, authContext, itemModel, null)
        {
            PropertiesAndValues = stepModel.PropertiesAndValues;
        }

        #region IWizardStep Members

        public override string Comment
        {
            get
            {
                string result = string.Format("Item will be created in catalog '{0}'".Localize(), InnerItem.Catalog.Name);
                var parentCategoryVM = stepModel.ParentEntityVM as ITreeCategoryViewModel;
                if (parentCategoryVM != null)
                    result += string.Format(", category '{0}'".Localize(), ((Category)parentCategoryVM.InnerItem).Name);

                return result;
            }
        }

        public override string Description
        {
            get { return "Enter Item details.".Localize(); }
        }

        public override bool IsValid
        {
            get
            {
                var retval = true;

                bool doNotifyChanges = false;

                InnerItem.Validate(doNotifyChanges);
                ValidatePropertySet(doNotifyChanges);

                if (InnerItem.Errors.ContainsKey("Name") ||
                    InnerItem.Errors.ContainsKey("PropertySet"))
                {
                    retval = false;
                    InnerItem.Errors.Clear();
                }
                return retval;
            }
        }

        #endregion

        #region ViewModelDetailBase members

        protected override void InitializePropertiesForViewing()
        {
            InitializeOverviewObjects();

            if (AllAvailableItemTypes.Count() == 1)
            {
                // setting initial value if only one choice exists.
                var propertySet = AllAvailableItemTypes.First();
                InnerItem.PropertySetId = propertySet.PropertySetId;
                InnerItem.PropertySet = propertySet;

                if (stepModel.InnerItemCatalogLanguages != null)
                    OnUIThread(() =>
                    {
                        CategoryViewModel.SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.ItemPropertyValues,
                                                     stepModel.InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
                    });
            }
        }

        protected override void SetSubscriptionUI()
        {
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }

        protected override void CloseSubscriptionUI()
        {
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
        }

        #endregion

        #region ISupportWizardPrepare members

        public void Prepare()
        {
            InnerItem.Code = Guid.NewGuid().ToString().Substring(0, 10);

            if (stepModel.ParentEntityVM is ITreeCategoryViewModel)
            {
                var parentCategoryVM = (ITreeCategoryViewModel)stepModel.ParentEntityVM;
                var relation = CreateEntity<CategoryItemRelation>();
                relation.CatalogId = InnerItem.CatalogId;
                relation.CategoryId = parentCategoryVM.InnerItem.CategoryId;
                InnerItem.CategoryItemRelations.Add(relation);
            }
        }

        #endregion

        #region private members

        protected override void InnerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PropertySetId")
            {
                CategoryViewModel.SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.ItemPropertyValues, stepModel.InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
            }
            else if (e.PropertyName == "IsBuyable")
            {
                stepModel.ParentWizard.RegisterPricingStep(InnerItem.IsBuyable);
            }
        }

        private void ValidatePropertySet(bool doNotifyChanges)
        {
            if (InnerItem.PropertySet == null || string.IsNullOrEmpty(InnerItem.PropertySet.PropertySetId))
                InnerItem.SetError("PropertySet", "PropertySet error".Localize(), doNotifyChanges);
            else
                InnerItem.ClearError("PropertySet");
        }

        #endregion
    }

    public class ItemPropertiesStepViewModel : ItemStepViewModel, IItemPropertiesStepViewModel, ISupportWizardPrepare
    {
        public ItemPropertiesStepViewModel(
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IViewModelsFactory<IPropertyValueBaseViewModel> vmFactory,
            ICatalogEntityFactory entityFactory,
            IAuthenticationContext authContext,
            ItemStepModel itemModel)
            : base(repositoryFactory, null, vmFactory, entityFactory, authContext, itemModel, null)
        {
            PropertiesAndValues = new ObservableCollection<PropertyAndPropertyValueBase>();
            stepModel.PropertiesAndValues = PropertiesAndValues;
        }

        #region IWizardStep Members

        public override string Comment
        {
            get
            {
                return "Enter property values. All properties marked required must have values in order for Item to be created.".Localize();
            }
        }

        public override string Description
        {
            get { return "Fill property values.".Localize(); }
        }

        public override bool IsValid
        {
            get
            {
                return PropertiesAndValues.All(x => x.IsValid);
            }
        }

        #endregion

        #region ViewModelDetailBase members

        protected override void InitializePropertiesForViewing()
        {
            OnUIThread(() =>
                {
                    InitializePropertiesAndValues();
                    stepModel.InnerItemCatalogLanguages = InnerItemCatalogLanguages;
                });
        }

        // override to don't do any action
        protected override void SetSubscriptionUI()
        {
        }

        protected override void CloseSubscriptionUI()
        {
        }

        #endregion

        #region ISupportWizardPrepare members

        public void Prepare()
        {
            // remove all property values that are not included in PropertySet
            var removeList = InnerItem.ItemPropertyValues.Where(
                x => InnerItem.PropertySet.PropertySetProperties.All(y => y.Property.Name != x.Name)).ToList();
            removeList.ForEach(x => InnerItem.ItemPropertyValues.Remove(x));
        }

        #endregion
    }

    public class ItemPricingStepViewModel : ItemStepViewModel, IItemPricingStepViewModel, ISupportWizardPrepare
    {
        public ItemPricingStepViewModel(
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IRepositoryFactory<IPricelistRepository> pricelistRepositoryFactory,
            ICatalogEntityFactory entityFactory,
            IAuthenticationContext authContext,
            ItemStepModel itemModel,
            IViewModelsFactory<IPriceViewModel> priceVmFactory)
            : base(repositoryFactory, pricelistRepositoryFactory, null, entityFactory, authContext, itemModel, priceVmFactory)
        {
            IsInitializingPricing = true;
        }

        #region IWizardStep Members

        public override string Description
        {
            get { return "Enter pricing information.".Localize(); }
        }

        public override bool IsValid
        {
            get { return true; }
        }

        #endregion

        #region ViewModelDetailBase members

        protected override void InitializePropertiesForViewing()
        {
            OnUIThread(InitializePricing);
        }

        // override to don't do any action
        protected override void SetSubscriptionUI()
        {
        }

        protected override void CloseSubscriptionUI()
        {
        }

        // override to don't do any action
        protected override void GetRepository()
        {
        }

        #endregion

        #region ISupportWizardPrepare members

        public void Prepare()
        {
            stepModel.PricelistRepository = _priceListRepository;
        }

        #endregion
    }
}
