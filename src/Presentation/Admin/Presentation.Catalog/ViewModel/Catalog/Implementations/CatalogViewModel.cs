using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class CatalogViewModel : ViewModelDetailAndWizardBase<catalogModel.Catalog>, ICatalogViewModel, IMultiSelectControlCommands
	{
		const string SettingName_Languages = "Languages";

		#region Dependencies

		private readonly ITreeCatalogViewModel _parentTreeVM;
		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;
		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
		private readonly IViewModelsFactory<IPropertyViewModel> _propertyVmFactory;
		private readonly IViewModelsFactory<IPropertySetViewModel> _propertySetVmFactory;
		private readonly INavigationManager _navManager;

		#endregion
		/// <summary>
		/// public. For viewing
		/// </summary>
		public CatalogViewModel(IViewModelsFactory<IPropertyViewModel> propertyVmFactory, IViewModelsFactory<IPropertySetViewModel> propertySetVmFactory,
			ITreeCatalogViewModel parentTreeVM,
			ICatalogEntityFactory entityFactory, catalogModel.Catalog item,
			IRepositoryFactory<ICatalogRepository> repositoryFactory,
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, INavigationManager navManager)
			: this(repositoryFactory, appConfigRepositoryFactory, entityFactory, item, false)
		{
			_parentTreeVM = parentTreeVM;
			_navManager = navManager;

			_propertyVmFactory = propertyVmFactory;
			_propertySetVmFactory = propertySetVmFactory;

			CurrentCatalogProperties = new ObservableCollection<Property>();
			ViewTitle = new ViewTitleBase
			{
                Title = "Catalog",
				SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
			};

			PropertyCreateCommand = new DelegateCommand(RaisePropertyCreateInteractionRequest);
			PropertyEditCommand = new DelegateCommand<Property>(RaisePropertyEditInteractionRequest, x => x != null);
			PropertyDeleteCommand = new DelegateCommand<Property>(RaisePropertyDeleteInteractionRequest, x => x != null);
			PropertySetCreateCommand = new DelegateCommand(RaisePropertySetCreateInteractionRequest);
			PropertySetEditCommand = new DelegateCommand<PropertySet>(RaisePropertySetEditInteractionRequest, x => x != null);

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		/// <summary>
		/// protected. For a step
		/// </summary>
		protected CatalogViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, ICatalogEntityFactory entityFactory, catalogModel.Catalog item)
			: this(repositoryFactory, appConfigRepositoryFactory, entityFactory, item, true)
		{
		}

		private CatalogViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, ICatalogEntityFactory entityFactory, catalogModel.Catalog item, bool isWizardMode)
			: base(entityFactory, item, isWizardMode)
		{
			_repositoryFactory = repositoryFactory;
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
		}

		public DelegateCommand PropertyCreateCommand { get; private set; }
		public DelegateCommand<Property> PropertyEditCommand { get; private set; }
		public DelegateCommand<Property> PropertyDeleteCommand { get; private set; }
		public DelegateCommand PropertySetCreateCommand { get; private set; }
		public DelegateCommand<PropertySet> PropertySetEditCommand { get; private set; }

		public ObservableCollection<Property> CurrentCatalogProperties { get; private set; }
		public ICatalogRepository ItemRepository { get { return (ICatalogRepository)Repository; } }

		#region ICatalogViewModel Members

		public ObservableCollection<CatalogLanguageDisplay> AllAvailableLanguages
		{
			get;
			private set;
		}
		public ObservableCollection<CatalogLanguageDisplay> InnerItemCatalogLanguages { get; private set; }

		#endregion

		#region ViewModelBase members

		public override string IconSource
		{
			get
			{
				return "Icon_Catalog";
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

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.CatalogId),
												NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}


		public void RaiseCanExecuteChanged()
		{
			PropertyEditCommand.RaiseCanExecuteChanged();
			PropertyDeleteCommand.RaiseCanExecuteChanged();
		}

		/// <summary>
		/// gets all languages for specified catalog. Function is almost duplicated in StoreViewModel
		/// </summary>
		/// <param name="catalogId"></param>
		/// <param name="_appConfigRepositoryFactory"></param>
		/// <returns></returns>
		internal static ObservableCollection<CatalogLanguageDisplay> GetAvailableLanguages(string catalogId, IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory)
		{
			var result = new ObservableCollection<CatalogLanguageDisplay>();
			using (var repository = _appConfigRepositoryFactory.GetRepositoryInstance())
			{
				var setting = repository.Settings.Where(x => x.Name == SettingName_Languages).ExpandAll().SingleOrDefault();
				if (setting != null)
				{
					foreach (var item in setting.SettingValues.Select(language => new CatalogLanguage { Language = language.ShortTextValue, CatalogId = catalogId }))
					{
						result.Add(new CatalogLanguageDisplay
							{
								Language = item,
								DisplayName = CultureInfo.GetCultureInfo(item.Language).DisplayName
							});
					}
				}
			}

			return result;
		}

		#endregion

		#region IMultiSelectControlCommands
		public void SelectItem(object selectedObj)
		{
			var selectedItem = (CatalogLanguageDisplay)selectedObj;
			var item = selectedItem.Language;
			InnerItem.CatalogLanguages.Add(item);
			InnerItemCatalogLanguages.Add(selectedItem);
		}

		public void SelectAllItems(ICollectionView availableItemsCollectionView)
		{
			var itemsList = availableItemsCollectionView.Cast<CatalogLanguageDisplay>().ToList();
			itemsList.ForEach(SelectItem);
		}

		public void UnSelectItem(object selectedObj)
		{
			var selectedItem = selectedObj as CatalogLanguageDisplay;
			// prevent removing default language
			if (selectedItem != null && !selectedItem.Language.Language.Equals(InnerItem.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase))
			{
				var item = InnerItem.CatalogLanguages.First(x => x.Language.Equals(selectedItem.Language.Language, StringComparison.InvariantCultureIgnoreCase));
				InnerItem.CatalogLanguages.Remove(item);
				InnerItemCatalogLanguages.Remove(selectedItem);
			}
		}

		public void UnSelectAllItems(System.Collections.IList currentListItems)
		{
			InnerItemCatalogLanguages.ToList().ForEach(UnSelectItem);
		}
		#endregion

		#region ViewModelDetailBase<T>

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override void SetSubscriptionUI()
		{
			InnerItem.CatalogLanguages.CollectionChanged += ViewModel_PropertyChanged;
		}

		protected override void CloseSubscriptionUI()
		{
			InnerItem.CatalogLanguages.CollectionChanged -= ViewModel_PropertyChanged;
		}

		protected override void LoadInnerItem()
		{
			var item = ItemRepository.Catalogs.OfType<catalogModel.Catalog>()
				.Where(x => x.CatalogId == OriginalItem.CatalogId)
				.Expand("CatalogLanguages, PropertySets/PropertySetProperties/Property/PropertyValues")
				.SingleOrDefault();

			OnUIThread(() => { InnerItem = item; });
		}

		protected override void InitializePropertiesForViewing()
		{
			if (AllAvailableLanguages == null)
			{
				AllAvailableLanguages = GetAvailableLanguages(InnerItem.CatalogId, _appConfigRepositoryFactory);
				OnPropertyChanged("AllAvailableLanguages");
			}

			// currently selected languages
			var InnerItemCatalogLanguageDisplays = AllAvailableLanguages.Where(x => InnerItem.CatalogLanguages.Any(y => y.Language.Equals(x.Language.Language, StringComparison.InvariantCultureIgnoreCase)));
			InnerItemCatalogLanguages = new ObservableCollection<CatalogLanguageDisplay>(InnerItemCatalogLanguageDisplays);
			OnPropertyChanged("InnerItemCatalogLanguages");

			if (!IsWizardMode)
			{
				// Initialize catalog Properties
				var allProperties = ItemRepository.Properties.ExpandAll().Where(x => x.CatalogId == InnerItem.CatalogId).ToList();
				OnUIThread(() =>
				{
					CurrentCatalogProperties.Clear();
					CurrentCatalogProperties.Add(allProperties);
				});

				OnPropertyChanged("CurrentCatalogProperties");
			}
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Catalog '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		public override string ExceptionContextIdentity { get { return string.Format("Catalog ({0})", DisplayName); } }

		protected override void AfterSaveChangesUI()
		{
			// just basic properties inject is enough. Injecting collections can generate repository errors.
			OriginalItem.InjectFrom(InnerItem);



			_parentTreeVM.RefreshUI();
		}

		#endregion

		#region private members

		private void RaisePropertyCreateInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<Property>();
			if (RaisePropertyEditInteractionRequest(item, "Create property".Localize()))
			{
				item.CatalogId = InnerItem.CatalogId;
				CurrentCatalogProperties.Add(item);
				Repository.Add(item);

				OnViewModelPropertyChangedUI(null, null);
			}
		}

		private void RaisePropertyEditInteractionRequest(Property originalItem)
		{
			var item = originalItem.DeepClone(EntityFactory as CatalogEntityFactory);
			if (RaisePropertyEditInteractionRequest(item, "Edit property".Localize()))
			{
				// copy all values to original:
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
				OnViewModelPropertyChangedUI(null, null);
			}
		}

		private void RaisePropertyDeleteInteractionRequest(Property originalItem)
		{
			var propertySets = InnerItem.PropertySets.Where(x => x.PropertySetProperties.Any(y => y.PropertyId == originalItem.PropertyId));
			var propertySetsNameString = string.Empty;
			propertySets.ToList().ForEach(x => propertySetsNameString += String.Format("'{0}',\r\n", x.Name));
			propertySetsNameString = !string.IsNullOrEmpty(propertySetsNameString) ? propertySetsNameString.Remove(propertySetsNameString.LastIndexOf(",")) : propertySetsNameString;
			var confirmation = new ConditionalConfirmation
			{
				Content = string.IsNullOrEmpty(propertySetsNameString) ? string.Format("Are you sure you want to delete Property '{0}'?".Localize(), originalItem.Name) : string.Format("Property '{0}' is used in \r\n{1}\r\nproperty set(s).\r\nAre you sure to delete it?".Localize(), originalItem.Name, propertySetsNameString),
				Title = "Delete".Localize(null, LocalizationScope.DefaultCategory)
			};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					originalItem.PropertyValues.Clear();
					Repository.Remove(originalItem);
					Repository.UnitOfWork.Commit();
					CurrentCatalogProperties.Remove(originalItem);
					OnViewModelPropertyChangedUI(null, null);
				}
			});

		}

		private bool RaisePropertyEditInteractionRequest(StorageEntity item, string title)
		{
			var result = false;
			var itemVM = _propertyVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("properties", CurrentCatalogProperties),
				new KeyValuePair<string, object>("parentCatalog", InnerItem)
				);
			var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = title, Content = itemVM };

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});

			return result;
		}

		private void RaisePropertySetCreateInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<PropertySet>();
			item.TargetType = PropertyTargetType.Sku.ToString(); // setting default value
			ICollectionChange<PropertySetProperty> itemsCollection;
			if (RaisePropertySetEditInteractionRequest(item, "Create property set".Localize(), out itemsCollection))
			{
				item.CatalogId = InnerItem.CatalogId;

				UpdatePropertySetProperties(itemsCollection, item);
				InnerItem.PropertySets.Add(item);
				OnViewModelPropertyChangedUI(null, null);
			}
		}

		private void RaisePropertySetEditInteractionRequest(PropertySet originalItem)
		{
			var item = originalItem.DeepClone(EntityFactory as CatalogEntityFactory);

			ICollectionChange<PropertySetProperty> itemsCollection;
			if (RaisePropertySetEditInteractionRequest(item, "Edit property set".Localize(), out itemsCollection))
			{
				// copy all values to original:
				OnUIThread(() =>
					{
						UpdatePropertySetProperties(itemsCollection, originalItem);
						//originalItem.InjectFrom<CloneInjection>(item);
						originalItem.InjectFrom(item);
					});
				OnViewModelPropertyChangedUI(null, null);
			}
		}

		private bool RaisePropertySetEditInteractionRequest(PropertySet item, string title, out ICollectionChange<PropertySetProperty> itemsCollection)
		{
			var result = false;
			var itemVM = _propertySetVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("properties", CurrentCatalogProperties));
			var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = title, Content = itemVM };

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});
			itemsCollection = itemVM.GetItemsCollection();

			return result;
		}

		public void UpdatePropertySetProperties(ICollectionChange<PropertySetProperty> itemsCollection, PropertySet parentItem)
		{
			var repository = Repository;
			foreach (var removedItem in itemsCollection.RemovedItems)
			{
				var item = parentItem.PropertySetProperties.FirstOrDefault(x => x.PropertySetPropertyId == removedItem.PropertySetPropertyId);
				if (item != null)
				{
					parentItem.PropertySetProperties.Remove(item);
				}
			}

			// priority could have changed
			foreach (var updatedItem in itemsCollection.UpdatedItems)
			{
				var item = parentItem.PropertySetProperties.SingleOrDefault(x => x.PropertySetPropertyId == updatedItem.PropertySetPropertyId);
				if (item != null)
				{
					item.InjectFrom(updatedItem);
				}
			}

			foreach (var addedItem in itemsCollection.AddedItems)
			{
				parentItem.PropertySetProperties.Add(addedItem);
			}

			var disposableCollection = itemsCollection as IDisposable;
			if (disposableCollection != null)
				disposableCollection.Dispose();
		}

		#endregion
	}
}