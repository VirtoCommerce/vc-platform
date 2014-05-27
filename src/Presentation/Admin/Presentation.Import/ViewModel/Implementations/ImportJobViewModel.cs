using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using PropertyChanged;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Repositories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Import.Model;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Import.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Implementations
{
	[ImplementPropertyChanged]
	public class ImportJobViewModel : ViewModelDetailAndWizardBase<ImportJob>, IImportJobViewModel
	{
		#region const
		private const string CustomString = "Custom";
		#endregion

		#region Dependencies
		//import service to execute retrieve selected file columns
		private readonly IImportService _importService;
		//for wizard purposes
		protected readonly WizardViewModelBare _parentViewModel;
		//needed to create ViewModels
		private readonly IViewModelsFactory<IPickAssetViewModel> _assetVmFactory;
		private readonly IViewModelsFactory<IColumnMappingViewModel> _mappingVmFactory;
		private readonly IAuthenticationContext _authContext;
		private readonly IRepositoryFactory<IImportRepository> _repositoryFactory;
		private readonly IRepositoryFactory<ICatalogRepository> _catalogRepositoryFactory;
		private readonly INavigationManager _navManager;
		private readonly string _homeMenuName;
		#endregion

		#region Properties
		public ImportEntityType[] EntityImporters { get; private set; }

		private static readonly ColumnDelimiter[] _columnDelimiters = new[] { 
				new ColumnDelimiter{ DisplayName = "Auto".Localize(), Value="?" },
				new ColumnDelimiter{ DisplayName = "Comma".Localize(), Value="," },
				new ColumnDelimiter{ DisplayName = "Semicolon".Localize(), Value=";" },
				new ColumnDelimiter{ DisplayName = "Tab".Localize(), Value="\t" },
				new ColumnDelimiter{ DisplayName = "Other".Localize(), Value=string.Empty }
			};

		public ColumnDelimiter[] ColumnDelimiters
		{
			get { return _columnDelimiters; }
		}

		private static readonly TextQualifier[] _textQualifiers = new[]
			{
				new TextQualifier {DisplayName = "Quote".Localize(), Value = '\''},
				new TextQualifier {DisplayName = "Other".Localize(), Value = ' '}
			};

		public TextQualifier[] TextQualifiers
		{
			get { return _textQualifiers; }
		}

		[AlsoNotifyFor("IsPropertySetSelectable")]
		public PropertySet[] PropertySets { get; set; }

		public CatalogBase[] Catalogs { get; set; }

		[DoNotNotify]
		public virtual string[] CsvFileColumns { get; set; }
		#endregion

		#region Controls availability properties

		public bool IsInEditMode
		{
			get { return IsWizardMode; }
		}

		public bool IsCustomDelimiterSelected
		{
			get
			{
				return !string.IsNullOrWhiteSpace(InnerItem.ColumnDelimiter) && !string.IsNullOrEmpty(InnerItem.ColumnDelimiter);
			}
		}

		public bool IsCatalogSelectable
		{
			get
			{
				return InnerItem.EntityImporter == ImportEntityType.Category.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.Product.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.Sku.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.Bundle.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.DynamicKit.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.Package.ToString();
			}
		}

		public bool IsTypeSelectable
		{
			get
			{
				return EntityImporters.Count() > 1;
			}
		}

		private bool _ignoreErrors;
		public bool IgnoreErrors
		{
			get
			{
				return _ignoreErrors;
			}
			set
			{
				_ignoreErrors = true;
				InnerItem.MaxErrorsCount = -1;
				OnPropertyChanged();
			}

		}

		public bool IsPropertySetSelectable
		{
			get
			{
				return PropertySets != null && PropertySets.Any() && (InnerItem.EntityImporter == ImportEntityType.Category.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.Product.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.Sku.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.Bundle.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.DynamicKit.ToString() ||
					InnerItem.EntityImporter == ImportEntityType.Package.ToString());
			}
		}

		public string ErrorText { get; set; }
		public bool IsError { get; set; }

		public void RaiseCanExecuteChanged()
		{
			ItemEditCommand.RaiseCanExecuteChanged();
			ItemClearCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region constructor
		public ImportJobViewModel(
			IRepositoryFactory<IImportRepository> repositoryFactory,
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
			IViewModelsFactory<IPickAssetViewModel> assetVmFactory,
			IViewModelsFactory<IColumnMappingViewModel> mappingVmFactory,
			IImportJobEntityFactory importJobFactory,
			INavigationManager navManager,
			ImportJob item,
			IImportService importService,
			ImportEntityType[] entityImporters,
			IAuthenticationContext authContext,
			string homeMenuName)
			: base(importJobFactory, item, false)
		{
			EntityImporters = entityImporters;
			_assetVmFactory = assetVmFactory;
			_mappingVmFactory = mappingVmFactory;
			_importService = importService;
			_authContext = authContext;
			_repositoryFactory = repositoryFactory;
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_navManager = navManager;
			_homeMenuName = homeMenuName;

			ViewTitle = new ViewTitleBase()
				{
                    Title = "Import job",
					SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""

				};

			OpenItemCommand = new DelegateCommand(() =>
			{
				if (true) //TODO set permission
				{
					_navManager.Navigate(NavigationData);
				}
			});
			CommandInit();
		}

		protected ImportJobViewModel(
			IRepositoryFactory<IImportRepository> repositoryFactory,
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
			IImportJobEntityFactory importJobFactory,
			ImportJob item,
			WizardViewModelBare parentVM,
			IViewModelsFactory<IPickAssetViewModel> assetVmFactory,
			IViewModelsFactory<IColumnMappingViewModel> mappingVmFactory,
			IImportService importService,
			ImportEntityType[] entityImporters,
			IAuthenticationContext authContext)
			: base(importJobFactory, item, true)
		{
			EntityImporters = entityImporters;
			_parentViewModel = parentVM;
			_assetVmFactory = assetVmFactory;
			_mappingVmFactory = mappingVmFactory;
			_importService = importService;
			_authContext = authContext;
			_repositoryFactory = repositoryFactory;
			_catalogRepositoryFactory = catalogRepositoryFactory;
			CommandInit();
		}

		#endregion

		#region Commands
		public DelegateCommand<MappingItem> ItemEditCommand { get; private set; }
		public DelegateCommand<MappingItem> ItemClearCommand { get; private set; }
		public DelegateCommand FilePickCommand { get; private set; }

		public DelegateCommand<PropertySet> UpdatePropertySetCommand { get; private set; }
		public DelegateCommand<CatalogBase> UpdateCatalogCommand { get; private set; }
		public DelegateCommand<object> UpdateImportTypeCommand { get; private set; }
		public DelegateCommand<ColumnDelimiter> UpdateColumnDelimiterCommand { get; private set; }
		#endregion

		#region Requests
		public InteractionRequest<Confirmation> CommonConfirmRequest2 { get; private set; }
		#endregion

		#region ViewModelBase Members

		public override string IconSource
		{
			get
			{
				return "Icon_ImportJob";
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
					(SolidColorBrush)Application.Current.TryFindResource("ImportJobDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		protected override void DoDuplicate()
		{
			var item = InnerItem.DeepClone(EntityFactory as IKnownSerializationTypes);
			item.ImportJobId = item.GenerateNewKey();
			item.PropertiesMap.ToList().ForEach(mapItem =>
				{
					mapItem.ImportJobId = item.ImportJobId;
					mapItem.MappingItemId = mapItem.GenerateNewKey();
					mapItem.ImportJob = null;
				});

			item.Name = item.Name + "_1";
			Repository.Add(item);
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.ImportJobId), _homeMenuName,
															NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Import job ({0})".Localize(), DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool HasPermission()
		{
			return _authContext.CheckPermission(PredefinedPermissions.CatalogCatalog_Import_JobsManage);
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		public override bool IsValid
		{
			get
			{
				var retVal = false;

				if (this is IImportJobOverviewStepViewModel || IsWizardMode)
				{
					retVal = !string.IsNullOrEmpty(InnerItem.EntityImporter)
						   && !string.IsNullOrEmpty(InnerItem.TemplatePath)
						   && !string.IsNullOrEmpty(InnerItem.Name);
				}

				if (this is IImportJobMappingStepViewModel || !IsWizardMode)
				{
					retVal =
						InnerItem.PropertiesMap.Where(req => req.IsRequired)
								 .All(prop => !string.IsNullOrEmpty(prop.CsvColumnName) || !string.IsNullOrEmpty(prop.CustomValue));
				}

				return retVal;
			}
		}

		public override string Description
		{
			get
			{
				if (this is IImportJobOverviewStepViewModel)
					return "Enter Import Job general information.".Localize();
				else
					return "Define properties mappings.".Localize();
			}
		}

		/// <summary>
		/// Return RefusedConfirmation for Cancel Confirm dialog
		/// </summary>
		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to import job {0}?'".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item = (Repository as IImportRepository).ImportJobs.Where(x => x.ImportJobId == OriginalItem.ImportJobId).Expand(x => x.PropertiesMap).SingleOrDefault();
			OnUIThread(() => InnerItem = item);
		}

		protected override void InitializePropertiesForViewing()
		{
			OnUIThread(() =>
				{
					//initialize available importers
					if (AvailableImporters == null)
					{
						AvailableImporters = new List<EntityImporterBase>
							{ 
								new ItemImporter(ImportEntityType.Product.ToString()), 
								new ItemImporter(ImportEntityType.Sku.ToString()), 
								new ItemImporter(ImportEntityType.Bundle.ToString()), 
								new ItemImporter(ImportEntityType.Package.ToString()), 
								new ItemImporter(ImportEntityType.DynamicKit.ToString()), 
								new CategoryImporter(), 
								new AssociationImporter(_catalogRepositoryFactory.GetRepositoryInstance()), 
								new RelationImporter(_catalogRepositoryFactory.GetRepositoryInstance()), 
								new LocalizationImporter(),
								new PriceImporter(_catalogRepositoryFactory.GetRepositoryInstance()),
								new ItemAssetImporter(),
								new TaxCategoryImporter(),
								new JurisdictionImporter(),
								new JurisdictionGroupImporter(),
								new TaxValueImporter(),
								new SeoImporter()
							}.ToArray();
					}

					//if this is wizard overview step or edit dialog
					if (this is IImportJobOverviewStepViewModel || !IsWizardMode)
					{
						if (Catalogs == null)
						{
							Catalogs = _catalogRepositoryFactory.GetRepositoryInstance().Catalogs.ToArray();
						}

						if (string.IsNullOrEmpty(InnerItem.EntityImporter) && EntityImporters != null)
						{
							InnerItem.EntityImporter = EntityImporters.First().ToString();
						}

						if (this is IImportJobOverviewStepViewModel)
						{
							if (InnerItem.EntityImporter == ImportEntityType.Category.ToString() ||
								InnerItem.EntityImporter == ImportEntityType.Product.ToString() ||
								InnerItem.EntityImporter == ImportEntityType.Bundle.ToString() ||
								InnerItem.EntityImporter == ImportEntityType.DynamicKit.ToString() ||
								InnerItem.EntityImporter == ImportEntityType.Package.ToString() ||
								InnerItem.EntityImporter == ImportEntityType.Sku.ToString())
								InnerItem.CatalogId = Catalogs.First().CatalogId;
						}

						UpdatePropertySets(InnerItem.PropertySetId);
						CsvFileColumns = GetCsvColumns();
						OnPropertyChanged("IsCatalogSelectable");
						OnPropertyChanged("IsTypeSelectable");
						OnPropertyChanged("IsPropertySetSelectable");
					}
				});
		}

		protected override void AfterSaveChangesUI()
		{
			OriginalItem.InjectFrom<CloneInjection>(InnerItem);
		}

		protected override void SetSubscriptionUI()
		{
			if (InnerItem.PropertiesMap != null)
			{
				InnerItem.PropertiesMap.ToList().ForEach(x => x.PropertyChanged += ViewModel_PropertyChanged);
				InnerItem.PropertiesMap.CollectionChanged += ViewModel_PropertyChanged;
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (InnerItem.PropertiesMap != null)
			{
				InnerItem.PropertiesMap.ToList().ForEach(x => x.PropertyChanged -= ViewModel_PropertyChanged);
				InnerItem.PropertiesMap.CollectionChanged -= ViewModel_PropertyChanged;
			}
		}

		#endregion

		#region private

		private void CommandInit()
		{
			FilePickCommand = new DelegateCommand(RaiseFilePickInteractionRequest);
			CommonConfirmRequest2 = new InteractionRequest<Confirmation>();

			UpdatePropertySetCommand = new DelegateCommand<PropertySet>(RaiseUpdatePropertySetRequest);
			UpdateCatalogCommand = new DelegateCommand<CatalogBase>(RaiseUpdateCatalogRequest);
			UpdateImportTypeCommand = new DelegateCommand<object>(RaiseUpdateImportTypeRequest);
			UpdateColumnDelimiterCommand = new DelegateCommand<ColumnDelimiter>(RaiseUpdateColumnDelimiterRequest);

			ItemEditCommand = new DelegateCommand<MappingItem>(RaiseItemEditInteractionRequest, x => x != null);
			ItemClearCommand = new DelegateCommand<MappingItem>(RaiseItemClearInteractionRequest, x => x != null);
		}


		private void RaiseUpdateColumnDelimiterRequest(ColumnDelimiter obj)
		{
			CsvFileColumns = GetCsvColumns();
			OnPropertyChanged("IsCustomDelimiterSelected");
		}

		private void RaiseUpdatePropertySetRequest(PropertySet obj)
		{
			if (IsCatalogSelectable && !string.IsNullOrEmpty(InnerItem.CatalogId))
			{
				using (var rep = _catalogRepositoryFactory.GetRepositoryInstance())
				{
					var catalogName =
						rep.Catalogs.Where(x => x.CatalogId == InnerItem.CatalogId).First().Name;
					var propertySetName = string.Empty;
					if (IsPropertySetSelectable)
					{
						var propertySet = rep.PropertySets.Where(x => x.PropertySetId == InnerItem.PropertySetId).FirstOrDefault();
						if (propertySet != null)
							propertySetName = propertySet.Name;
					}
					InnerItem.Name = string.Format("{0} {1} {2} import".Localize(), catalogName, propertySetName, InnerItem.EntityImporter);
				}
			}
		}

		private void RaiseUpdateImportTypeRequest(object obj)
		{
			UpdatePropertySets(null);
			OnPropertyChanged("IsCatalogSelectable");
			OnPropertyChanged("IsTypeSelectable");
			OnPropertyChanged("IsPropertySetSelectable");
			if (IsCatalogSelectable && !string.IsNullOrEmpty(InnerItem.CatalogId))
			{
				using (var rep = _catalogRepositoryFactory.GetRepositoryInstance())
				{
					var catalogName =
						rep.Catalogs.Where(x => x.CatalogId == InnerItem.CatalogId).First().Name;
					var propertySetName = string.Empty;
					if (IsPropertySetSelectable)
					{
						var propertySet = rep.PropertySets.Where(x => x.PropertySetId == InnerItem.PropertySetId).FirstOrDefault();
						if (propertySet != null)
							propertySetName = propertySet.Name;
					}
					InnerItem.Name = string.Format("{0} {1} {2} import".Localize(), catalogName, propertySetName, InnerItem.EntityImporter);
				}
			}
			else
			{
				InnerItem.Name = string.Format("{0} import".Localize(), InnerItem.EntityImporter);
			}
			OnPropertyChanged("InnerItem");
		}

		private void RaiseUpdateCatalogRequest(CatalogBase obj)
		{
			if (obj != null)
				InnerItem.Name = string.Format("{0} {1} import".Localize(), obj.Name, InnerItem.EntityImporter);
			UpdatePropertySets(null);
		}

		private EntityImporterBase[] AvailableImporters;

		private string[] GetCsvColumns()
		{
			string[] retVal = null;


			if (_importService.Exists(InnerItem.TemplatePath))
			{
				if (!string.IsNullOrEmpty(InnerItem.TemplatePath))
				{
					if (InnerItem.ColumnDelimiter == "?")
						InnerItem.ColumnDelimiter = _importService.GetCsvColumnsAutomatically(InnerItem.TemplatePath);

					retVal = _importService.GetCsvColumns(InnerItem.TemplatePath /*csv file name*/, InnerItem.ColumnDelimiter);
				}
			}
			else
			{
				IsError = true;
				ErrorText = string.Format("File '{0}' doesn't exist".Localize(), InnerItem.TemplatePath);
			}
			return retVal;
		}

		private void SetMappingItems()
		{
			//add system properties of the selected import type (importer)
			var tmp = AvailableImporters.FirstOrDefault(x => x.Name == InnerItem.EntityImporter);
			if (tmp != null)
			{
				var newList = new ObservableCollection<MappingItem>();
				tmp.SystemProperties.ToList().ForEach(sysProp => newList.Add(new MappingItem
					{
						EntityColumnName = sysProp.Name,
						IsSystemProperty = true,
						IsRequired = sysProp.IsRequiredProperty,
						DisplayName = sysProp.IsRequiredProperty ? string.Format("* {0}", sysProp.DisplayName) : sysProp.DisplayName,
						ImportJobId = InnerItem.ImportJobId,
						CustomValue = !string.IsNullOrEmpty(sysProp.DefaultValue) ? sysProp.DefaultValue : null,
						ImportJob = InnerItem
					}));

				InnerItem.PropertiesMap = new ObservableCollection<MappingItem>(newList.OrderBy(item => item.DisplayName));
			}

			//add custom properties (if any in the selected property set
			if ((tmp != null && (tmp.Name == ImportEntityType.Product.ToString() || tmp.Name == ImportEntityType.Sku.ToString() || tmp.Name == ImportEntityType.Bundle.ToString() || tmp.Name == ImportEntityType.DynamicKit.ToString() || tmp.Name == ImportEntityType.Package.ToString() || tmp.Name == ImportEntityType.Category.ToString())) && InnerItem.PropertySetId != null && InnerItem.CatalogId != null)
			{
				//get available locales for the catalog
				var localesQuery = _catalogRepositoryFactory.GetRepositoryInstance().Catalogs
				.OfType<Catalog>()
				.Where(x => x.CatalogId == InnerItem.CatalogId)
				.Expand(x => x.CatalogLanguages)
				.SingleOrDefault();

				var locales = new List<CatalogLanguage>();
				if (localesQuery != null)
					locales = localesQuery.CatalogLanguages.ToList();

				//get property set properties
				var ps =
					_catalogRepositoryFactory.GetRepositoryInstance()
											 .PropertySets.Expand("PropertySetProperties/Property")
											 .Where(x => x.PropertySetId == InnerItem.PropertySetId)
											 .FirstOrDefault();
				var props = ps != null ? ps.PropertySetProperties : null;
				if (props != null)
				{
					var newList = new ObservableCollection<MappingItem>();
					props.ToList().ForEach(prop =>
						{
							if (prop.Property.IsLocaleDependant)
							{
								locales.ForEach(
									locale =>
									newList.Add(new MappingItem
										{
											EntityColumnName = prop.Property.Name,
											DisplayName = prop.Property.IsRequired ? string.Format("* {0}", prop.Property.Name) : prop.Property.Name,
											IsRequired = prop.Property.IsRequired,
											Locale = locale.Language,
											IsSystemProperty = false,
											ImportJobId = InnerItem.ImportJobId,
											ImportJob = InnerItem
										}));
							}
							else
							{
								newList.Add(new MappingItem
									{
										EntityColumnName = prop.Property.Name,
										IsSystemProperty = false,
										DisplayName = prop.Property.IsRequired ? string.Format("* {0}", prop.Property.Name) : prop.Property.Name,
										IsRequired = prop.Property.IsRequired,
										ImportJobId = InnerItem.ImportJobId,
										ImportJob = InnerItem
									});
							}
						});
					InnerItem.PropertiesMap.Add(newList.OrderBy(item => item.DisplayName));
				}
			}

			//default columns mapping
			if (CsvFileColumns != null && CsvFileColumns.Length > 0)
			{
				InnerItem.PropertiesMap.ToList().ForEach(col => CsvFileColumns.ToList().ForEach(csvcolumn =>
					{
						//if entity column name contains csv column name or visa versa - match entity property name to csv file column name
						if (col.EntityColumnName.ToLower().Contains(csvcolumn.ToLower()) ||
							csvcolumn.ToLower().Contains(col.EntityColumnName.ToLower()))
						{
							InnerItem.PropertiesMap.First(x => x.EntityColumnName == col.EntityColumnName).CsvColumnName = csvcolumn;
							InnerItem.PropertiesMap.First(x => x.EntityColumnName == col.EntityColumnName).CustomValue = null;
						}
					}));
			}
		}

		private void RaiseFilePickInteractionRequest()
		{
			var itemVM = _assetVmFactory.GetViewModelInstance();
			itemVM.AssetPickMode = true;
			itemVM.RootItemId = null;

			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(itemVM.Validate) { Content = itemVM, Title = "Select file".Localize(null, LocalizationScope.DefaultCategory) },
				(x) =>
				{
					if (x.Confirmed)
					{
						InnerItem.TemplatePath = itemVM.SelectedAsset.FolderItemId;
						CsvFileColumns = GetCsvColumns();
						SetMappingItems();
					}
				});
		}

		private void RaiseItemEditInteractionRequest(MappingItem originalItem)
		{
			var item = originalItem.DeepClone(EntityFactory as ImportJobEntityFactory);
			var selectedProperty = AvailableImporters.First(importer => importer.Name == InnerItem.EntityImporter).SystemProperties.FirstOrDefault(prop => prop.Name == item.EntityColumnName);
			var param = new ColumnMappingEntity
							{
								MappingItem = item,
								CsvColumnsList = CsvFileColumns,
								ImportProperty = selectedProperty
							};

			var itemVM = _mappingVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", param)
				);

			var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = "Edit column mapping".Localize(), Content = itemVM };

			CommonConfirmRequest2.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					var innerItemProp = InnerItem.PropertiesMap.FirstOrDefault(prop => prop.EntityColumnName == item.EntityColumnName && prop.Locale == item.Locale);

					if (innerItemProp != null)
					{
						// copy all values to original:
						if (!string.IsNullOrEmpty(item.CustomValue) && (item.CsvColumnName == CustomString || string.IsNullOrEmpty(item.CsvColumnName)))
						{
							innerItemProp.Locale = item.Locale;
							originalItem.CustomValue = innerItemProp.CustomValue = item.CustomValue;
							originalItem.CsvColumnName = innerItemProp.CsvColumnName = null;
							originalItem.StringFormat = innerItemProp.StringFormat = item.StringFormat;
						}
						else
						{
							innerItemProp.Locale = item.Locale;
							originalItem.CsvColumnName = innerItemProp.CsvColumnName = item.CsvColumnName;
							originalItem.CustomValue = innerItemProp.CustomValue = null;
							originalItem.StringFormat = innerItemProp.StringFormat = item.StringFormat;
						}
					}

					OnIsValidChanged();
				}
			});
		}

		private void RaiseItemClearInteractionRequest(MappingItem originalItem)
		{
			//TODO check system properties map and custom properties map to clear property
			//InnerItem.ColumnMapping.PropertiesMap.First(prop => prop.EntityColumnName == originalItem.EntityColumnName).CustomValue = null;
			//originalItem.CsvColumnName = null;
			//originalItem.CustomValue = null;

			////fake mapping list update to refresh view
			//SetMappingItems();
		}

		private async void UpdatePropertySets(string Id)
		{
			if (!string.IsNullOrEmpty(InnerItem.CatalogId))
			{
				var propSets = await Task.Run(() => _catalogRepositoryFactory.GetRepositoryInstance().PropertySets.ExpandAll().Where(x => x.CatalogId == InnerItem.CatalogId && (x.TargetType == "All" || x.TargetType == InnerItem.EntityImporter)));

				PropertySets = propSets.ToArray();
				if (PropertySets.Any() && string.IsNullOrEmpty(Id))
				{
					InnerItem.PropertySetId = PropertySets.First().PropertySetId;
				}

				SetMappingItems();
			}
		}

		#endregion

		#region IWizardStep Members

		public override bool IsLast
		{
			get
			{
				return this is IImportJobMappingStepViewModel;
			}
		}

		#endregion

	}
}
