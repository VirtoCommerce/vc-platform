using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
    public class CreateCategoryViewModel : WizardContainerStepsViewModel, ICreateCategoryViewModel
    {
        public CreateCategoryViewModel(IViewModelsFactory<ICategoryPropertiesStepViewModel> propertiesVmFactory, IViewModelsFactory<ICategoryOverviewStepViewModel> overviewVmFactory, Category item)
        {
            item.StartDate = DateTime.Today;
            var _itemModel = new CategoryStepModel
            {
                InnerItem = item
            };

            var allParameters = new[] { new KeyValuePair<string, object>("itemModel", _itemModel) };

            // properties Step must be created first
            var propertiesStep = propertiesVmFactory.GetViewModelInstance(allParameters);
            // this step is created second, but registered first
            RegisterStep(overviewVmFactory.GetViewModelInstance(allParameters));

            // properties Step is registered second
            RegisterStep(propertiesStep);
        }
    }

    public class CategoryStepModel
    {
        public Category InnerItem;
        public ObservableCollection<PropertyAndPropertyValueBase> PropertiesAndValues;
        public List<string> InnerItemCatalogLanguages;
    }

    public abstract class CategoryStepViewModel : CategoryViewModel
    {
        protected CategoryStepModel stepModel;

        protected CategoryStepViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> vmFactory, ICatalogEntityFactory entityFactory, CategoryStepModel itemModel)
            : base(repositoryFactory, vmFactory, entityFactory, itemModel.InnerItem, itemModel.InnerItem.Catalog)
        {
            stepModel = itemModel;
        }
    }

    public class CategoryOverviewStepViewModel : CategoryStepViewModel, ICategoryOverviewStepViewModel
    {
        public CategoryOverviewStepViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> vmFactory, ICatalogEntityFactory entityFactory, CategoryStepModel itemModel)
            : base(repositoryFactory, vmFactory, entityFactory, itemModel)
        {
            PropertiesAndValues = stepModel.PropertiesAndValues;
        }

        #region IWizardStep Members

        public override bool IsValid
        {
            get
            {
                bool doNotifyChanges = false;
                InnerItem.Validate(doNotifyChanges);
                ValidatePropertySet(doNotifyChanges);

                var retval = InnerItem.Errors.Count == 0;
                InnerItem.Errors.Clear();

                //if (InnerItem.Errors.ContainsKey("Code") ||
                //	InnerItem.Errors.ContainsKey("Name") ||
                //	InnerItem.Errors.ContainsKey("PropertySet") ||
                //	InnerItem.StartDate == DateTime.MinValue)
                //	retval = false;

                return retval;
            }
        }

        public override string Comment
        {
            get
            {
                var result = string.Format("Category will be created in catalog '{0}'".Localize(), InnerItem.Catalog.CatalogId);
                var parentCategory = InnerItem.ParentCategory as Category;
                if (parentCategory != null)
                {
                    result += string.Format(", category '{0}'".Localize(), parentCategory.Name);
                }

                return result;
            }
        }

        public override string Description
        {
            get
            {
                return "Enter main category information.".Localize();
            }
        }

        #endregion

        #region ViewModelDetailBase members

        protected override void InitializePropertiesForViewing()
        {
            InitializePropertySets();

            if (AvailableCategoryTypes.Count() == 1)
            {
                // setting initial value if only one choice exists.
                var propertySet = AvailableCategoryTypes.First();
                InnerItem.PropertySetId = propertySet.PropertySetId;
                InnerItem.PropertySet = propertySet;

                if (stepModel.InnerItemCatalogLanguages != null)
                    OnUIThread(() =>
                    {
                        SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.CategoryPropertyValues,
                                                 stepModel.InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
                    });
            }
        }

        #endregion

        #region private members

        private void ValidatePropertySet(bool doNotifyChanges)
        {
            if (InnerItem.PropertySet == null || string.IsNullOrEmpty(InnerItem.PropertySet.PropertySetId))
                InnerItem.SetError("PropertySet", "PropertySet error".Localize(), doNotifyChanges);
            else
                InnerItem.ClearError("PropertySet");
        }

        protected override void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    {

                        const string invalidCodeCharacters = @"$+;=%{}[]|\/@ ~#!^*&()?:'<>,";

                        var name = InnerItem.Name;

                        //replace invalid characters in the name with - if any
                        foreach (var ch in invalidCodeCharacters.ToCharArray())
                        {
                            name = name.Replace(ch, '-');
                        }

                        // code gets a value of Name initially
                        InnerItem.Code = name;
                    }
                    break;
                case "PropertySetId":
                    SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.CategoryPropertyValues, stepModel.InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
                    break;
            }
        }

        #endregion
    }

    public class CategoryPropertiesStepViewModel : CategoryStepViewModel, ICategoryPropertiesStepViewModel, ISupportWizardPrepare
    {
        public CategoryPropertiesStepViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> vmFactory, ICatalogEntityFactory entityFactory, CategoryStepModel itemModel)
            : base(repositoryFactory, vmFactory, entityFactory, itemModel)
        {
            PropertiesAndValues = new ObservableCollection<PropertyAndPropertyValueBase>();
            stepModel.PropertiesAndValues = PropertiesAndValues;
        }

        #region IWizardStep Members

        public override bool IsValid
        {
            get
            {
                return PropertiesAndValues.All(x => x.IsValid);
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
                return "Enter property values. All properties marked required must have values in order for category to be created.".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "Enter property values.".Localize();
            }
        }

        #endregion

        #region ViewModelDetailBase members

        protected override void InitializePropertiesForViewing()
        {
            InitializePropertiesAndValues();
            stepModel.InnerItemCatalogLanguages = InnerItemCatalogLanguages;
        }

        protected override void SetSubscriptionUI()
        {
            //InnerItem.CategoryPropertyValues.CollectionChanged += ViewModel_PropertyChanged;
        }

        protected override void CloseSubscriptionUI()
        {
            //InnerItem.CategoryPropertyValues.CollectionChanged -= ViewModel_PropertyChanged;
        }

        #endregion

        public void Prepare()
        {
            // remove all property values that are not included in PropertySet
            var removeList = InnerItem.CategoryPropertyValues.Where(
                x => InnerItem.PropertySet.PropertySetProperties.All(y => y.Property.Name != x.Name)).ToList();
            removeList.ForEach(x => InnerItem.CategoryPropertyValues.Remove(x));
        }
    }
}
