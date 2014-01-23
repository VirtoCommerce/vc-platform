using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class CategoryViewModel : ViewModelDetailAndWizardBase<Category>, ICategoryViewModel
	{
		private readonly ITreeCategoryViewModel _parentTreeVM;
		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;
		private readonly IViewModelsFactory<IPropertyValueBaseViewModel> _propertyValueVmFactory;
		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
		private readonly INavigationManager _navManager;

		private readonly CatalogBase _parentCatalog;

		/// <summary>
		/// public. For viewing
		/// </summary>
		public CategoryViewModel(IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> propertyValueVmFactory, ICatalogEntityFactory entityFactory,
			IRepositoryFactory<ICatalogRepository> repositoryFactory, Category item,
			ITreeCategoryViewModel parentTreeVM, INavigationManager navManager)
			: this(appConfigRepositoryFactory, repositoryFactory, propertyValueVmFactory, entityFactory, item, CatalogHomeViewModel.GetCatalog(parentTreeVM), false)
		{
			_parentTreeVM = parentTreeVM;
			_navManager = navManager;

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
		protected CategoryViewModel(IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IRepositoryFactory<ICatalogRepository> repositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> propertyValueVmFactory, ICatalogEntityFactory entityFactory, Category item, CatalogBase parentCatalog)
			: this(appConfigRepositoryFactory, repositoryFactory, propertyValueVmFactory, entityFactory, item, parentCatalog, true)
		{
		}

		private CategoryViewModel(IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IRepositoryFactory<ICatalogRepository> repositoryFactory, IViewModelsFactory<IPropertyValueBaseViewModel> propertyValueVmFactory, ICatalogEntityFactory entityFactory, Category item, CatalogBase parentCatalog, bool isWizardMode)
			: base(entityFactory, item, isWizardMode)
		{
			_propertyValueVmFactory = propertyValueVmFactory;
			_repositoryFactory = repositoryFactory;
			_parentCatalog = parentCatalog;
			_appConfigRepositoryFactory = appConfigRepositoryFactory;

			PropertiesLocalesFilterCommand = new DelegateCommand<string>(RaisePropertiesLocalesFilter);
			SeoLocalesFilterCommand = new DelegateCommand<string>(RaiseSeoLocalesFilter);
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
						T valueItem;
						if (x.IsLocaleDependant)
						{
							locales.ForEach(y =>
							{
								valueItem = PropertyValues.FirstOrDefault(z => z.Name == x.Name && z.Locale == y);
								PropertiesAndValues.Add(new PropertyAndPropertyValueBase { Property = x, Value = valueItem, Locale = y });
							});
						}
						else
						{
							valueItem = PropertyValues.FirstOrDefault(z => z.Name == x.Name);
							PropertiesAndValues.Add(new PropertyAndPropertyValueBase { Property = x, Value = valueItem });
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

		internal static void RaisePropertyValueEditInteractionRequest<T>(IViewModelsFactory<IPropertyValueBaseViewModel> _vmFactory, InteractionRequest<Confirmation> confirmRequest, ICatalogEntityFactory entityFactory, Action<PropertyAndPropertyValueBase, PropertyAndPropertyValueBase> finalAction, PropertyAndPropertyValueBase originalItem) where T : PropertyValueBase
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

			if (originalItem.IsMultiValue && originalItem.Values == null)
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
				new KeyValuePair<string, object>("item", item)
				);

			var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = "Edit property value", Content = itemVM };

			confirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					finalAction(originalItem, item);
				}
			});
		}

		internal static bool ValidatePropertiesAndValues(ObservableCollection<PropertyAndPropertyValueBase> propertiesAndValues)
		{
			return propertiesAndValues.All(x => (x.Value != null && !string.IsNullOrEmpty(x.Value.ToString())) || !x.Property.IsRequired);
		}

		#region ICategoryViewModel Members

		public IEnumerable<PropertySet> AvailableCategoryTypes { get; private set; }
		public DelegateCommand<object> PropertyValueEditCommand { get; private set; }
		public DelegateCommand<object> PropertyValueDeleteCommand { get; private set; }

		private const int TabIndexOverview = 0;
		private const int TabIndexProperties = 1;

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

			InitializeSeoKeywords();
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
		}

		protected override void CloseSubscriptionUI()
		{
			InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
			//InnerItem.CategoryPropertyValues.CollectionChanged -= ViewModel_PropertyChanged;
		}

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
					InnerItem.SetError("Code", "A category with this Code already exists in this catalog", true);
					SelectedTabIndex = TabIndexOverview;
					isCodeValid = false;
				}
			}

			var isPropertyValuesValid = ValidatePropertiesAndValues(PropertiesAndValues);
			if (!isPropertyValuesValid && isCodeValid)
			{
				SelectedTabIndex = TabIndexProperties;
			}

			//seo is valid if was modified, seo keyword is not empty, or if all properties ar empty
			var allSeoValid = !_seoModified || SeoKeywords.All(keyword => !string.IsNullOrEmpty(keyword.Keyword) || (string.IsNullOrEmpty(keyword.ImageAltDescription) && string.IsNullOrEmpty(keyword.Title) && string.IsNullOrEmpty(keyword.MetaDescription)));

			return result && isPropertyValuesValid && isCodeValid && allSeoValid;
		}

		protected override void AfterSaveChangesUI()
		{
			UpdateSeoKeywords();
			// just basic properties inject is enough. Injecting collections can generate repository errors.
			OriginalItem.InjectFrom(InnerItem);
			_parentTreeVM.RefreshUI();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = "Save changes to Category '" + DisplayName + "'?",
				Title = "Action confirmation"
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
			if (e.PropertyName == "PropertySetId")
			{
				SetupPropertiesAndValues(InnerItem.PropertySet, InnerItem.CategoryPropertyValues, InnerItemCatalogLanguages, PropertiesAndValues, IsWizardMode);
			}
		}
		#endregion

		#region Properties tab

		public bool IsCatalogLanguageFilteringEnabled { get { return _parentCatalog is catalogModel.Catalog; } }
		public string FilterLanguage { get; private set; }
		public List<string> InnerItemCatalogLanguages { get; private set; }
		public ObservableCollection<PropertyAndPropertyValueBase> PropertiesAndValues { get; protected set; }
		public DelegateCommand<string> PropertiesLocalesFilterCommand { get; private set; }

		protected void InitializePropertiesAndValues()
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
				InnerItemCatalogLanguages = new List<string> { _parentCatalog.DefaultLanguage };
			}

			OnUIThread(() =>
			{
				OnPropertyChanged("InnerItemCatalogLanguages");

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

		public List<SeoUrlKeyword> SeoKeywords { get; private set; }

		private SeoUrlKeyword _currentSeoKeyword;
		public SeoUrlKeyword CurrentSeoKeyword 
		{
			get { return _currentSeoKeyword; } 
			set 
			{ 
				_currentSeoKeyword = value;
				OnPropertyChanged("CurrentSeoKeyword");
			} 
		}
		
		private bool _useDefaultMetaDescription = true;
		public bool UseDefaultMetaDescription
		{
			get { return _useDefaultMetaDescription && (CurrentSeoKeyword != null && string.IsNullOrEmpty(CurrentSeoKeyword.MetaDescription)); }
			set
			{
				_useDefaultMetaDescription = value;
				if (value && !string.IsNullOrEmpty(CurrentSeoKeyword.MetaDescription))
					CurrentSeoKeyword.MetaDescription = null;
				OnPropertyChanged("UseDefaultMetaDescription");
			}
		}

		private bool _useDefaultTitle = true;
		public bool UseDefaultTitle
		{
			get { return _useDefaultTitle && (CurrentSeoKeyword != null && string.IsNullOrEmpty(CurrentSeoKeyword.Title)); }
			set
			{
				_useDefaultTitle = value;
				if (value && !string.IsNullOrEmpty(CurrentSeoKeyword.Title))
					CurrentSeoKeyword.Title = null;
				OnPropertyChanged("UseDefaultTitle");
			}
		}

		private bool _useDefaultImageText = true;
		public bool UseDefaultImageText
		{
			get { return _useDefaultImageText && (CurrentSeoKeyword != null && string.IsNullOrEmpty(CurrentSeoKeyword.ImageAltDescription)); }
			set 
			{ 
				_useDefaultImageText = value;
				if (value && !string.IsNullOrEmpty(CurrentSeoKeyword.ImageAltDescription))
					CurrentSeoKeyword.ImageAltDescription = null;
				OnPropertyChanged("UseDefaultImageText");
			}
		}

		private void InitializeSeoKeywords()
		{
			using (var _appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
			{
				SeoKeywords =
					_appConfigRepository.SeoUrlKeywords.Where(
						keyword =>
						keyword.KeywordValue.Equals(InnerItem.Code) && keyword.KeywordType.Equals((int) SeoUrlKeywordTypes.Category))
					                    .ToList();
			}
			// filter values by locale
			SeoLocalesFilterCommand.Execute(_parentCatalog.DefaultLanguage);
			
		}

		private void RaiseSeoLocalesFilter(string locale)
		{
			//detach property changed
			if (CurrentSeoKeyword != null)
				CurrentSeoKeyword.PropertyChanged -= CurrentSeoKeyword_PropertyChanged;

			CurrentSeoKeyword =
				SeoKeywords.FirstOrDefault(keyword => keyword.Language.Equals(locale, StringComparison.InvariantCultureIgnoreCase));
			
			if (CurrentSeoKeyword == null)
			{
				CurrentSeoKeyword = new SeoUrlKeyword { Language = locale, IsActive = true, KeywordType = (int)SeoUrlKeywordTypes.Category, KeywordValue = InnerItem.Code, Created = DateTime.UtcNow };
				SeoKeywords.Add(CurrentSeoKeyword);
			}

			//attach property changed
			CurrentSeoKeyword.PropertyChanged += CurrentSeoKeyword_PropertyChanged;

			FilterSeoLanguage = locale;
			OnPropertyChanged("FilterSeoLanguage");

			_useDefaultTitle = true;
			OnPropertyChanged("UseDefaultTitle");

			_useDefaultMetaDescription = true;
			OnPropertyChanged("UseDefaultMetaDescription");

			_useDefaultImageText = true;
			OnPropertyChanged("UseDefaultImageText");
		}

		void CurrentSeoKeyword_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			_seoModified = true;
			OnViewModelPropertyChangedUI(null, null);
		}

		private void UpdateSeoKeywords()
		{
			//if any SEO keyword modified update or add it
			if (_seoModified)
			{
				using (var appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
				{
					SeoKeywords.ForEach(keyword =>
						{
							if (!string.IsNullOrEmpty(keyword.Keyword))
							{
								var originalKeyword =
									appConfigRepository.SeoUrlKeywords.Where(
										seoKeyword =>
										seoKeyword.KeywordValue.Equals(keyword.KeywordValue) && seoKeyword.Language.Equals(keyword.Language))
									                   .FirstOrDefault();

								if (originalKeyword != null)
								{
									originalKeyword.InjectFrom(keyword);
									appConfigRepository.Update(originalKeyword);
								}
								else
								{
									var addKeyword = new SeoUrlKeyword();
									addKeyword.InjectFrom(keyword);
									appConfigRepository.Add(addKeyword);
								}
							}
						});
					appConfigRepository.UnitOfWork.Commit();
				}
				_seoModified = false;
			}
		}

		private bool _seoModified;
		public string FilterSeoLanguage { get; private set; }
		public DelegateCommand<string> SeoLocalesFilterCommand { get; private set; }

		#endregion

		#region private members

		private void RaisePropertyValueEditInteractionRequest(object originalItemObject)
		{
			RaisePropertyValueEditInteractionRequest<CategoryPropertyValue>(_propertyValueVmFactory, CommonConfirmRequest, (ICatalogEntityFactory)EntityFactory, OnItemValueConfirmed, (PropertyAndPropertyValueBase)originalItemObject);
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
				{
					item.Values.ToList().ForEach(value => InnerItem.CategoryPropertyValues.Add(new CategoryPropertyValue { CategoryId = InnerItem.CategoryId, Name = item.PropertyName, KeyValue = value.PropertyValueId, ValueType = item.PropertyValueType }));
					originalItem.Values = new ObservableCollection<PropertyValueBase>();
					item.Values.ToList().ForEach(value => originalItem.Values.Add(new PropertyValue { Name = item.PropertyName, KeyValue = value.PropertyValueId, ValueType = item.PropertyValueType }));
				}
				else
				{
					List<PropertyValue> listToRemove = null;
					foreach (var val in originalItem.Values)
					{
						if (!item.Values.Any(y => y.PropertyValueId == val.PropertyValueId))
						{
							if (listToRemove == null)
								listToRemove = new List<PropertyValue>();
							listToRemove.Add((PropertyValue)val);
						}
					}
					if (listToRemove != null)
					{
						listToRemove.ForEach(x =>
						{
							var itemToRemove = InnerItem.CategoryPropertyValues.First(y => y.KeyValue == x.PropertyValueId);
							InnerItem.CategoryPropertyValues.Remove(itemToRemove);
							originalItem.Values.Remove(x);
						});
					}

					foreach (var val in item.Values)
					{
						if (originalItem.Values.All(y => y.PropertyValueId != val.PropertyValueId))
						{
							originalItem.Values.Add(val);
							var newValue = new CategoryPropertyValue { CategoryId = InnerItem.CategoryId, Name = item.PropertyName, KeyValue = val.PropertyValueId, ValueType = item.PropertyValueType };
							InnerItem.CategoryPropertyValues.Add(newValue);
						}
					}
				}
			}

			// for GUI update
			originalItem.Value = originalItem.Value ?? item.Value;
			OnViewModelCollectionChangedUI(null, null);
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
		}

		#endregion

	}
}
