using System;
using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Implementations;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Implementations
{
    public class CreateContentPublishingItemViewModel : WizardContainerStepsViewModel, ICreateContentPublishingItemViewModel
    {
        #region Dependencies

        private readonly IViewModelsFactory<IContentPublishingOverviewStepViewModel> _overviewVmFactory;
        private readonly IViewModelsFactory<IContentPublishingContentPlacesStepViewModel> _placesVmFactory;
        private readonly IViewModelsFactory<IContentPublishingDynamicContentStepViewModel> _contentVmFactory;
        private readonly IViewModelsFactory<IContentPublishingConditionsStepViewModel> _conditionsVmFactory;

        #endregion

        public CreateContentPublishingItemViewModel(
            IViewModelsFactory<IContentPublishingOverviewStepViewModel> overviewVmFactory,
            IViewModelsFactory<IContentPublishingContentPlacesStepViewModel> placesVmFactory,
            IViewModelsFactory<IContentPublishingDynamicContentStepViewModel> contentVmFactory,
            IViewModelsFactory<IContentPublishingConditionsStepViewModel> conditionsVmFactory,
            DynamicContentPublishingGroup item)
        {
            _overviewVmFactory = overviewVmFactory;
            _placesVmFactory = placesVmFactory;
            _conditionsVmFactory = conditionsVmFactory;
            _contentVmFactory = contentVmFactory;

            CreateWizardSteps(item);
        }

        private void CreateWizardSteps(DynamicContentPublishingGroup item)
        {
            var itemParameter = new KeyValuePair<string, object>("item", item);

            RegisterStep(_overviewVmFactory.GetViewModelInstance(itemParameter));
            RegisterStep(_placesVmFactory.GetViewModelInstance(itemParameter));
            RegisterStep(_contentVmFactory.GetViewModelInstance(itemParameter));
            RegisterStep(_conditionsVmFactory.GetViewModelInstance(itemParameter));
        }

    }

    public class ContentPublishingOverviewStepViewModel : ContentPublishingItemViewModel,
        IContentPublishingOverviewStepViewModel
    {
        public ContentPublishingOverviewStepViewModel(IRepositoryFactory<IDynamicContentRepository> repositoryFactory,
            IDynamicContentEntityFactory entityFactory, DynamicContentPublishingGroup item)
            : base(null, null, null, repositoryFactory, null, entityFactory, item)
        {

        }

        protected override void InitializePropertiesForViewing()
        {

        }


        #region IWizardStep

        public override bool IsValid
        {
            get
            {
                var retval = true;
                if (this is IContentPublishingOverviewStepViewModel)
                {
                    bool doNotifyChanges = false;

                    InnerItem.Validate(doNotifyChanges);

                    if (InnerItem.Errors.Count > 0 ||
                        String.IsNullOrEmpty(InnerItem.Name))
                    {
                        retval = false;
                        InnerItem.Errors.Clear();
                    }
                }

                return retval;
            }
        }

        public override bool IsLast
        {
            get
            {
                return false;
            }
        }

        public override string Comment
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Description
        {
            get
            {
                return string.Format("Enter Content publishing group details".Localize());
            }
        }

        #endregion
    }

    public class ContentPublishingContentPlacesStepViewModel : ContentPublishingItemViewModel,
        IContentPublishingContentPlacesStepViewModel, ISupportWizardPrepare
    {
        public ContentPublishingContentPlacesStepViewModel(
            IRepositoryFactory<IDynamicContentRepository> repositoryFactory,
            IDynamicContentEntityFactory entityFactory, DynamicContentPublishingGroup item)
            : base(null, null, null, repositoryFactory, null, entityFactory, item)
        {

        }

        protected override void InitializePropertiesForViewing()
        {
            InitializeContentPlaces();
        }

        #region IWizardStep

        public override bool IsValid
        {
            get
            {
                var retval = true;
                if (this is IContentPublishingContentPlacesStepViewModel)
                {
                    bool doNotifyChanges = false;
                    InnerItem.Validate(doNotifyChanges);

                    if (InnerItemContentPlaces == null || InnerItemContentPlaces.Count == 0)
                        retval = false;
                }

                return retval;
            }
        }

        public override bool IsLast
        {
            get
            {
                return false;
            }
        }

        public override string Comment
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Description
        {
            get
            {
                return "Select content places".Localize();
            }
        }

        #endregion

        public void Prepare()
        {
            UpdateContentPlacesItems();
        }
    }

    public class ContentPublishingDynamicContentStepViewModel : ContentPublishingItemViewModel,
        IContentPublishingDynamicContentStepViewModel, ISupportWizardPrepare
    {
        public ContentPublishingDynamicContentStepViewModel(
            IRepositoryFactory<IDynamicContentRepository> repositoryFactory,
            IDynamicContentEntityFactory entityFactory, DynamicContentPublishingGroup item)
            : base(null, null, null, repositoryFactory, null, entityFactory, item)
        {

        }

        protected override void InitializePropertiesForViewing()
        {
            InitializeDynamicContent();
        }

        #region IWizardStep

        public override bool IsValid
        {
            get
            {
                var retval = true;

                if (this is IContentPublishingDynamicContentStepViewModel)
                {
                    InnerItem.Validate(false);

                    if (InnerItemDynamicContent == null || InnerItemDynamicContent.Count == 0)
                        retval = false;
                }

                return retval;
            }
        }

        public override bool IsLast
        {
            get
            {
                return false;
            }
        }

        public override string Comment
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Description
        {
            get
            {
                return "Select dynamic content".Localize();
            }
        }

        #endregion

        public void Prepare()
        {
            UpdateDynamicContentItems();
        }
    }

    public class ContentPublishingConditionsStepViewModel : ContentPublishingItemViewModel,
        IContentPublishingConditionsStepViewModel, ISupportWizardPrepare
    {
        public ContentPublishingConditionsStepViewModel(
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
            IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
            IViewModelsFactory<ISearchCategoryViewModel> searchCategoryVmFactory,
            IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
            IRepositoryFactory<IDynamicContentRepository> repositoryFactory,
            IDynamicContentEntityFactory entityFactory,
            DynamicContentPublishingGroup item)
            : base(appConfigRepositoryFactory, countryRepositoryFactory, searchCategoryVmFactory, repositoryFactory, storeRepositoryFactory, entityFactory, item)
        {

        }

        protected override void InitializePropertiesForViewing()
        {
            InitializeExpressionElementBlock();
        }

        public void Prepare()
        {
            UpdateFromExpressionElementBlock();
        }


        #region IWizardStep

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        public override bool IsLast
        {
            get
            {
                return true;
            }
        }

        public override string Comment
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Description
        {
            get
            {
                return "Set availability conditions".Localize();
            }
        }

        #endregion
    }
}
