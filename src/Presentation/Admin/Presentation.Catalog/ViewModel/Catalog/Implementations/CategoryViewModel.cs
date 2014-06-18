using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class CategoryViewModel : ViewModelDetailAndWizardBase<Category>, ICategoryViewModel
    {
        #region Dependencies
        #endregion
        private readonly ITreeCategoryViewModel _parentTreeVM;
        private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;
        private readonly IViewModelsFactory<IPropertyValueBaseViewModel> _propertyValueVmFactory;
        private readonly IViewModelsFactory<ICategorySeoViewModel> _seoVmFactory;
        private readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;
        private readonly INavigationManager _navManager;

        protected readonly CatalogBase _parentCatalog;

        /// <summary>
        /// public. For viewing
        /// </summary>
        public CategoryViewModel(IRepositoryFactory<IStoreRepository> storeRepositoryFactory, IViewModelsFactory<ICategorySeoViewModel> seoVmFactory, IViewModelsFactory<IPropertyValueBaseViewModel> propertyValueVmFactory, ICatalogEntityFactory entityFactory,
            IRepositoryFactory<ICatalogRepository> repositoryFactory, Category item,
            ITreeCategoryViewModel parentTreeVM, INavigationManager navManager)
            : this(repositoryFactory, propertyValueVmFactory, entityFactory, item, CatalogHomeViewModel.GetCatalog(parentTreeVM), false)
        {
            _parentTreeVM = parentTreeVM;
            _navManager = navManager;
            _seoVmFactory = seoVmFactory;
            _storeRepositoryFactory = storeRepositoryFactory;

            ViewTitle = new ViewTitleBase
                {
                    Title = "Category",
                    SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
                };

            PropertiesAndValues = new ObservableCollection<PropertyAndPropertyValueBase>();

            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
        }

        /// <summary>
        /// protected. For a step
        /// </summary>
        protected CategoryViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> propertyValueVmFactory, ICatalogEntityFactory entityFactory, Category item, CatalogBase parentCatalog)
            : this(repositoryFactory, propertyValueVmFactory, entityFactory, item, parentCatalog, true)
        {
        }

        private CategoryViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> propertyValueVmFactory, ICatalogEntityFactory entityFactory, Category item, CatalogBase parentCatalog, bool isWizardMode)
            : base(entityFactory, item, isWizardMode)
        {
            _propertyValueVmFactory = propertyValueVmFactory;
            _repositoryFactory = repositoryFactory;
            _parentCatalog = parentCatalog;

            PropertiesLocalesFilterCommand = new DelegateCommand<string>(RaisePropertiesLocalesFilter);
            PropertyValueEditCommand = new DelegateCommand<object>(RaisePropertyValueEditInteractionRequest, x => x != null);
            PropertyValueDeleteCommand = new DelegateCommand<object>(RaisePropertyValueDeleteInteractionRequest, x => x != null);
        }

        public ICatalogRepository ItemRepository { get { return (ICatalogRepository)Repository; } }

        public void RaiseCanExecuteChanged()
        {
            PropertyValueEditCommand.RaiseCanExecuteChanged();
            PropertyValueDeleteCommand.RaiseCanExecuteChanged();
        }

        internal static void SetupPropertiesAndValues<T>(PropertySet PropertySet, ObservableCollection<T> PropertyValues, List<string> locales, ObservableCollection<PropertyAndPropertyValueBase> PropertiesAndValues, bool isWizardMode) where T : PropertyValueBase
        {
            PropertiesAndValues.Clear();
            if (PropertySet != null)
            {
                // generate value objects from PropertySet.PropertySetProperties and fill current values
                PropertySet.PropertySetProperties.Select(x => x.Property).ToList().ForEach(x =>
                    {
                        if (x.IsLocaleDependant)
                        {
                            locales.ForEach(y =>
                            {
                                var item = new PropertyAndPropertyValueBase { Property = x, Locale = y };
                                if (x.IsMultiValue)
                                {
                                    var values = PropertyValues.Where(z => z.Name == x.Name && z.Locale == y);
                                    item.Values = new ObservableCollection<PropertyValueBase>(values);
                                }
                                else
                                {
                                    item.Value = PropertyValues.FirstOrDefault(z => z.Name == x.Name && z.Locale == y);
                                }

                                PropertiesAndValues.Add(item);
                            });
                        }
                        else
                        {
                            var item = new PropertyAndPropertyValueBase { Property = x };
                            if (x.IsMultiValue)
                            {
                                var values = PropertyValues.Where(z => z.Name == x.Name);
                                item.Values = new ObservableCollection<PropertyValueBase>(values);
                            }
                            else
                            {
                                item.Value = PropertyValues.FirstOrDefault(z => z.Name == x.Name);
                            }

                            PropertiesAndValues.Add(item);
                        }
                    });
            }

            // fill values that misses properties. Only in edit dialog
            if (!isWizardMode)
                PropertyValues.ToList().ForEach(x =>
                           {
                               var placeholderForValue = PropertiesAndValues.FirstOrDefault(y => y.PropertyName == x.Name);
                               if (placeholderForValue == null)
                               {
                                   PropertiesAndValues.Add(new PropertyAndPropertyValueBase { Value = x });
                               }
                               //else
                               //{
                               //    if (placeholderForValue.IsMultiValue)
                               //    {
                               //        if (placeholderForValue.Values == null)
                               //            placeholderForValue.Values = new ObservableCollection<PropertyValueBase>();
                               //        placeholderForValue.Values.Add(placeholderForValue.Property.PropertyValues.First(y => y.PropertyValueId == x.KeyValue));
                               //    }
                               //    else
                               //        placeholderForValue.Value = x;
                               //}
                           });
        }

        internal static void RaisePropertyValueEditInteractionRequest<T>(IViewModelsFactory<IPropertyValueBaseViewModel> _vmFactory, InteractionRequest<Confirmation> confirmRequest, ICatalogEntityFactory entityFactory, Action<PropertyAndPropertyValueBase, PropertyAndPropertyValueBase> finalAction, PropertyAndPropertyValueBase originalItem, string locale) where T : PropertyValueBase
        {
            var item = originalItem.DeepClone(entityFactory as CatalogEntityFactory);

            T itemValue;

            if (!originalItem.IsMultiValue)
            {
                item.Values = new ObservableCollection<PropertyValueBase>();
                if (originalItem.Value == null)
                {
                    itemValue = (T)entityFactory.CreateEntityForType(typeof(T));
                    if (originalItem.Property != null)
                        itemValue.ValueType = originalItem.Property.PropertyValueType;
                    item.Value = itemValue;
                }
            }
            else if (originalItem.Values == null)
            {
                //itemValue = (T)entityFactory.CreateEntityForType(typeof(T));
                // item.CategoryId = InnerItem.CategoryId;
                //if (originalItem.Property != null)
                //	itemValue.ValueType = originalItem.Property.PropertyValueType;
                item.Values = new ObservableCollection<PropertyValueBase>();
            }
            //else
            //{
            //	itemValue = (T)originalItem.Value.DeepClone(entityFactory as CatalogEntityFactory);
            //}


            var itemVM = _vmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("locale", locale));

            var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = "Edit property value".Localize(), Content = itemVM };

            confirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    finalAction(originalItem, item);
                }
            });
        }

        #region ICategoryViewModel Members

        public IEnumerable<PropertySet> AvailableCategoryTypes { get; private set; }
        public DelegateCommand<object> PropertyValueEditCommand { get; private set; }
        public DelegateCommand<object> PropertyValueDeleteCommand { get; private set; }

        private const int TabIndexOverview = 0;
        private const int TabIndexProperties = 1;
        private const int TabIndexSeo = 2;

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            protected set { _selectedTabIndex = value; OnPropertyChanged(); }
        }

        #endregion

        #region ViewModelBase members

        public override string IconSource
        {
            get
            {
                return "Icon_Category";
            }
        }

        public override string DisplayName
        {
            get
            {
                return OriginalItem.Name;
            }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                var result =
                  (SolidColorBrush)Application.Current.TryFindResource("CatalogDetailItemMenuBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        protected override void InitializePropertiesForViewing()
        {
            InitializePropertySets();

            InitializePropertiesAndValues();

            InitSeoStep();
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                return _navigationData ??
                       (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.CategoryId), NavigationNames.HomeName, NavigationNames.MenuName, this));
            }
        }

        #endregion

        #region ViewModelDetailBase members
        public override string ExceptionContextIdentity { get { return string.Format("Category ({0})", DisplayName); } }

        protected override void GetRepository()
        {
            Repository = _repositoryFactory.GetRepositoryInstance();
        }

        protected override void LoadInnerItem()
        {
            var item = ItemRepository.Categories
                                .Where(x => x.CategoryId == OriginalItem.CategoryId).OfType<Category>()
                                .Expand(x => x.CategoryPropertyValues)
                                .Expand("PropertySet/PropertySetProperties/Property/PropertyValues")
                                .SingleOrDefault();

            OnUIThread(() => { InnerItem = item; });
        }

        protected override void SetSubscriptionUI()
        {
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
            //InnerItem.CategoryPropertyValues.CollectionChanged += ViewModel_PropertyChanged;

            if (SeoStepViewModel != null)
            {
                if (SeoStepViewModel.SeoKeywords != null)
                    SeoStepViewModel.SeoKeywords.ForEach(keyword => keyword.PropertyChanged += ViewModel_PropertyChanged);
            }
        }

        protected override void CloseSubscriptionUI()
        {
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
            //InnerItem.CategoryPropertyValues.CollectionChanged -= ViewModel_PropertyChanged;

            if (SeoStepViewModel != null)
            {
                if (SeoStepViewModel.SeoKeywords != null)
                    SeoStepViewModel.SeoKeywords.ForEach(keyword => keyword.PropertyChanged -= ViewModel_PropertyChanged);
            }
        }

        // function almost duplicated in ItemViewModel
        protected override bool IsValidForSave()
        {
            var result = InnerItem.Validate();

            // Code should be unique in scope of catalog
            var isCodeValid = true;
            if (InnerItem.Code != OriginalItem.Code)
            {
                var count = ItemRepository.Categories
                                          .Where(x =>
                                              x.CatalogId == InnerItem.CatalogId && x.Code == InnerItem.Code && x.CategoryId != InnerItem.CategoryId)
                                          .Count();

                if (count > 0)
                {
                    InnerItem.SetError("Code", "A category with this Code already exists in this catalog".Localize(), true);
                    SelectedTabIndex = TabIndexOverview;
                    isCodeValid = false;
                }
            }

            var isPropertyValuesValid = PropertiesAndValues.All(x => x.IsValid);
            if (!isPropertyValuesValid && isCodeValid)
            {
                SelectedTabIndex = TabIndexProperties;
                var val = PropertiesAndValues.First(x => !x.IsValid);
                if (!string.IsNullOrEmpty(val.Locale) && val.Locale != FilterLanguage)
                {
                    RaisePropertiesLocalesFilter(val.Locale);
                }
            }

            var seoIsValid = true;
            if (SeoStepViewModel != null)
            {
                seoIsValid = SeoStepViewModel.IsValid;
                if (!seoIsValid)
                    SelectedTabIndex = TabIndexSeo;
            }

            return result && isPropertyValuesValid && isCodeValid && seoIsValid;
        }

        protected override void AfterSaveChangesUI()
        {
            if (SeoStepViewModel != null)
            {
                SeoStepViewModel.SaveSeoKeywordsChanges();
            }

            // just basic properties inject is enough. Injecting collections can generate repository errors.
            OriginalItem.InjectFrom(InnerItem);
            _parentTreeVM.RefreshUI();
        }

        protected override RefusedConfirmation CancelConfirm()
        {
            return new RefusedConfirmation
            {
                Content = string.Format("Save changes to Category '{0}'?".Localize(), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }
        #endregion

        #region Overview tab

        protected void InitializePropertySets()
        {
            if (AvailableCategoryTypes == null)
            {
                // query PropertySets
                var allPropertySets = ItemRepository.PropertySets
                    .Where(x => x.TargetType == PropertyTargetType.All.ToString() || x.TargetType == PropertyTargetType.Category.ToString())
                    .Expand("PropertySetProperties/Property/PropertyValues");

                var innerItemCatalog = _parentCatalog as catalogModel.Catalog;
                if (innerItemCatalog != null)
                {
                    // query PropertySets only from current real catalog
                    allPropertySets = allPropertySets.Where(x => x.CatalogId == innerItemCatalog.CatalogId);
                }
                // else: this is Category in a Virtual Catalog

                AvailableCategoryTypes = allPropertySets.ToList();
                OnPropertyChanged("AvailableCategoryTypes");
            }
        }

        protected virtual void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Code")
                SeoStepViewModel.ChangeKeywordValue(InnerItem.Code);
            if (e.PropertyName == "PropertySetId")
            {
                SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.CategoryPropertyValues, InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
            }
        }
        #endregion

        #region Properties tab

        public bool IsCatalogLanguageFilteringEnabled { get { return InnerItemCatalogLanguages != null && InnerItemCatalogLanguages.Count() > 1; } }
        public string FilterLanguage { get; private set; }
        public List<string> InnerItemCatalogLanguages { get; protected set; }
        public ObservableCollection<PropertyAndPropertyValueBase> PropertiesAndValues { get; protected set; }
        public DelegateCommand<string> PropertiesLocalesFilterCommand { get; private set; }

        // function almost duplicated in ItemViewModel
        protected void InitializePropertiesAndValues()
        {
            if (IsWizardMode)
            {
                InnerItemCatalogLanguages = new List<string>();
                InnerItemCatalogLanguages.Add(_parentCatalog.DefaultLanguage);
            }
            else
            {
                var innerItemCatalog = _parentCatalog as catalogModel.Catalog;
                if (innerItemCatalog != null)
                {
                    // query catalog languages
                    if (innerItemCatalog.CatalogLanguages.Count == 0)
                    {
                        var catalogLanguages = ItemRepository.Catalogs
                            .OfType<catalogModel.Catalog>()
                            .Where(x => x.CatalogId == innerItemCatalog.CatalogId)
                            .Expand(x => x.CatalogLanguages)
                            .Single()
                            .CatalogLanguages.ToList();
                        innerItemCatalog.CatalogLanguages.Add(catalogLanguages);
                    }
                    InnerItemCatalogLanguages = innerItemCatalog.CatalogLanguages.Select(x => x.Language).ToList();
                }
                else
                {
                    InnerItemCatalogLanguages = new List<string>();

                    // _storeRepositoryFactory is null in a wizard. That's ok, no need for all the languages
                    if (_storeRepositoryFactory != null)
                    {
                        using (var storeRepository = _storeRepositoryFactory.GetRepositoryInstance())
                        {
                            var languages =
                                storeRepository.Stores.Where(store => store.Catalog == _parentCatalog.CatalogId)
                                                .Expand(store => store.Languages).ToList();

                            var customComparer = new PropertyComparer<StoreLanguage>("LanguageCode");
                            var lang = languages.SelectMany(x => x.Languages).Distinct(customComparer);

                            foreach (var l in lang)
                            {
                                InnerItemCatalogLanguages.Add(l.LanguageCode);
                            }
                        }
                    }

                    if (!InnerItemCatalogLanguages.Any(x => x.Equals(_parentCatalog.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase)))
                        InnerItemCatalogLanguages.Add(_parentCatalog.DefaultLanguage);
                }
            }

            OnUIThread(() =>
            {
                OnPropertyChanged("InnerItemCatalogLanguages");
                OnPropertyChanged("IsCatalogLanguageFilteringEnabled");

                // filter values by locale
                PropertiesLocalesFilterCommand.Execute(_parentCatalog.DefaultLanguage);

                SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.CategoryPropertyValues, InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
            });
        }



        // function duplicated in ItemViewModel
        private void RaisePropertiesLocalesFilter(string locale)
        {
            var view = CollectionViewSource.GetDefaultView(PropertiesAndValues);
            view.Filter = (x =>
            {
                var propertyAndPropertyValue = (PropertyAndPropertyValueBase)x;
                //var result = (propertyAndPropertyValue.Value == null
                //			&& (!propertyAndPropertyValue.Property.IsLocaleDependant || !PropertiesAndValues.Any(z => z.Value != null && z.Value.Name == propertyAndPropertyValue.Property.Name && z.Value.Locale == locale)))
                //		|| (propertyAndPropertyValue.Value != null
                //			&& (!propertyAndPropertyValue.Property.IsLocaleDependant || string.IsNullOrEmpty(propertyAndPropertyValue.Value.Locale) || propertyAndPropertyValue.Value.Locale.Equals(locale, StringComparison.InvariantCultureIgnoreCase)));
                var result = string.IsNullOrEmpty(propertyAndPropertyValue.Locale) || propertyAndPropertyValue.Locale.Equals(locale, StringComparison.InvariantCultureIgnoreCase);
                return result;
            });

            FilterLanguage = locale;
            OnPropertyChanged("FilterLanguage");
        }

        #endregion

        #region SEO tab

        public ICategorySeoViewModel SeoStepViewModel { get; private set; }

        protected void InitSeoStep()
        {
            var itemParameter = new KeyValuePair<string, object>("item", InnerItem);
            var languagesParameter = new KeyValuePair<string, object>("languages", InnerItemCatalogLanguages);
            var parentCat = new KeyValuePair<string, object>("parentCatalog", _parentCatalog);
            SeoStepViewModel =
                    _seoVmFactory.GetViewModelInstance(itemParameter, languagesParameter, parentCat);
            OnPropertyChanged("SeoStepViewModel");
        }

        #endregion

        #region private members

        private void RaisePropertyValueEditInteractionRequest(object originalItemObject)
        {
            RaisePropertyValueEditInteractionRequest<CategoryPropertyValue>(_propertyValueVmFactory, CommonConfirmRequest, (ICatalogEntityFactory)EntityFactory, OnItemValueConfirmed, (PropertyAndPropertyValueBase)originalItemObject, FilterLanguage);
        }

        // function almost duplicated in ItemViewModel
        private void OnItemValueConfirmed(PropertyAndPropertyValueBase originalItem, PropertyAndPropertyValueBase item)
        {
            if (!item.IsMultiValue)
            {
                if (originalItem.Property != null)
                {
                    item.Value.Name = originalItem.Property.Name;
                    item.Value.Locale = originalItem.Locale;
                }

                if (originalItem.Value == null)
                {
                    ((CategoryPropertyValue)item.Value).CategoryId = InnerItem.CategoryId;
                    InnerItem.CategoryPropertyValues.Add((CategoryPropertyValue)item.Value);
                }
                else
                    UpdatePropertyValue(item.Value);
            }
            else
            {
                if (originalItem.Values == null)
                    originalItem.Values = new ObservableCollection<PropertyValueBase>();

                var listToRemove = originalItem.Values.Where(val => item.Values.All(y => y.PropertyValueId != val.PropertyValueId)).ToList();
                listToRemove.ForEach(x =>
                {
                    var itemToRemove = InnerItem.CategoryPropertyValues.First(y => y.KeyValue == x.KeyValue);
                    InnerItem.CategoryPropertyValues.Remove(itemToRemove);
                    originalItem.Values.Remove(x);
                });

                foreach (var value in item.Values)
                {
                    if (originalItem.Values.All(y => y.PropertyValueId != value.PropertyValueId))
                    {
                        InnerItem.CategoryPropertyValues.Add(new CategoryPropertyValue
                        {
                            CategoryId = InnerItem.CategoryId,
                            Name = item.PropertyName,
                            Locale = value.Locale,
                            KeyValue = value.PropertyValueId,
                            ValueType = item.PropertyValueType
                        });
                        originalItem.Values.Add(new PropertyValue
                        {
                            Name = item.PropertyName,
                            Locale = value.Locale,
                            KeyValue = value.PropertyValueId,
                            ValueType = item.PropertyValueType
                        });
                    }
                }
            }

            // for GUI update
            originalItem.Value = originalItem.Value ?? item.Value;
            OnViewModelPropertyChangedUI(null, null);
        }

        // function almost duplicated in ItemViewModel
        private void UpdatePropertyValue(PropertyValueBase newValue)
        {
            //TODO update value according to ValueType
            var item = InnerItem.CategoryPropertyValues.FirstOrDefault(x => x.PropertyValueId == newValue.PropertyValueId);
            if (item == null)
            {
                item = (CategoryPropertyValue)EntityFactory.CreateEntityForType(typeof(CategoryPropertyValue));
                InnerItem.CategoryPropertyValues.Add(item);
            }
            item.InjectFrom(newValue);
            item.CategoryId = InnerItem.CategoryId;
        }

        // function almost duplicated in ItemViewModel
        private void RaisePropertyValueDeleteInteractionRequest(object originalItemObject)
        {
            var originalItem = (PropertyAndPropertyValueBase)originalItemObject;
            var item = originalItem.Value as CategoryPropertyValue;
            if (item != null)
            {
                InnerItem.CategoryPropertyValues.Remove(item);
                if (originalItem.Property == null)
                {
                    PropertiesAndValues.Remove(originalItem);
                }
                else
                {
                    originalItem.Value = null;
                }

                OnViewModelCollectionChangedUI(null, null);
            }
            else if (originalItem.IsMultiValue && originalItem.Values != null)
            {
                foreach (var value in originalItem.Values)
                {
                    var itemToRemove = InnerItem.CategoryPropertyValues.First(y => y.KeyValue == value.KeyValue);
                    InnerItem.CategoryPropertyValues.Remove(itemToRemove);
                }

                originalItem.Values = null;
                OnViewModelCollectionChangedUI(null, null);
            }
        }

        #endregion

    }
}
