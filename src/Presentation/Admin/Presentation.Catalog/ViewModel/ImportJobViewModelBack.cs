using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Unity;
using Presentation.Catalog.ViewModel.Wizard;
using Presentation.Core.Infrastructure;
using Presentation.Core.Infrastructure.Navigation;
using Presentation.Core.Infrastructure.Wizard;
using VirtoSoftware.CommerceFoundation.Catalogs.Factories;
using VirtoSoftware.CommerceFoundation.Catalogs.Model;
using VirtoSoftware.CommerceFoundation.Frameworks;
using VirtoSoftware.CommerceFoundation.Frameworks.Extensions;
using VirtoSoftware.CommerceFoundation.Importing.Model;
using VirtoSoftware.CommerceFoundation.Importing.Repositories;
using VirtoSoftware.CommerceFoundation.Catalogs.Repositories;
using Omu.ValueInjecter;
using VirtoSoftware.CommerceFoundation.Frameworks.ConventionInjections;
using Presentation.Catalog.Model;
using System.Collections.Generic;
using Presentation.Asset.ViewModel;
using VirtoSoftware.CommerceFoundation.Importing.Services;

namespace Presentation.Catalog.ViewModel
{
    public class ImportJobViewModelBack : WizardStepViewModelBase, IImportJobViewModel, IClosable, ISupportAcceptChanges, IMinimizable, IOpenTracking, ISupportDelayInitialization
	{
		#region privates
		private ImportJob _originalItem;
        private ICatalogEntityFactory _entityFactory;
        private IImportRepository _itemRepository;
		private IImportService _importService;
        protected WizardViewModelBare _parentViewModel;
		#endregion

		public EntityImporterBase[] AllAvailableEntityImporters { get; private set; }
		public ObservableCollection<ColumnMapping> AllAvailableColumnMappings { get; private set; }
		public ColumnDelimiter[] AllAvailableColumnDelimiters { get; private set; }
		public TextQualifier[] AllAvailableTextQualifiers { get; private set; }
		public CatalogBase[] AllAvailableCatalogs { get; private set; }
		public PropertySet[] AllAvailablePropertySets { get; private set; }
		public string[] CsvFileColumns { get; set; }

		public DelegateCommand<EntityImporterBase> UpdateImporterCommand { get; private set; }
		public DelegateCommand<PropertySet> UpdatePropertySetCommand { get; private set; }

        public ImportJobViewModelBack(IUnityContainer container, ICatalogEntityFactory entityFactory, ImportJob item)
            : this(container, entityFactory, item, null, true)
        {
			ViewTitle = new ViewTitleBase() { Title = "Import job", SubTitle = DisplayName.ToUpper() };
        }

        protected ImportJobViewModelBack(IUnityContainer container, ICatalogEntityFactory entityFactory, ImportJob item, WizardViewModelBare parentVM, bool isSingleDialogEditing)
        {
            Container = container;
            _entityFactory = entityFactory;
            InnerItem = _originalItem = item;
            _parentViewModel = parentVM;
            IsSingleDialogEditing = isSingleDialogEditing;
			_importService = Container.Resolve<IImportService>();
			_itemRepository = Container.Resolve<IImportRepository>();

			if (isSingleDialogEditing)
            {
                _originalItem = InnerItem;

                OpenItemCommand = new DelegateCommand(() =>
                {
                    var navigationmanager = container.Resolve<NavigationManager>();
                    NavigationData = new NavigationItem(InnerItem.ImportJobId, NavigationNames.HomeName, NavigationNames.MenuName, this);
                    navigationmanager.Navigate(NavigationData);
                });

                CancelCommand = new DelegateCommand<object>(OnCancelCommand);
                SaveChangesCommand = new DelegateCommand<object>((x) => OnSaveChangesCommand(), (x) => { return IsModified; });
                MinimizeCommand = new DelegateCommand(() => MinimizableViewRequestedEvent(this, null));
            }
            else
            {
                InitializePropertiesForViewing();				
            }

			FilePickCommand = new DelegateCommand(RaiseFilePickInteractionRequest);
			CreateMappingCommand = new DelegateCommand(RaiseCreateMappingInteractionRequest);
			CancelConfirmRequest = new InteractionRequest<Confirmation>();
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			CommonConfirmRequest2 = new InteractionRequest<Confirmation>();

			UpdateImporterCommand = new DelegateCommand<EntityImporterBase>((x) => OnImporterChangesCommand(x));
			UpdatePropertySetCommand = new DelegateCommand<PropertySet>((x) => OnPropertySetChangesCommand(x));
			
			CatalogChangedCommand = new DelegateCommand<CatalogBase>((x) => OnCatalogChangesCommand(x));

			ItemEditCommand = new DelegateCommand<MappingItem>((x) => RaiseItemEditInteractionRequest(x), x => x != null);
			ItemClearCommand = new DelegateCommand<MappingItem>((x) => RaiseItemClearInteractionRequest(x), x => x != null);
        }

		
		public DelegateCommand<MappingItem> ItemEditCommand { get; private set; }
		public DelegateCommand<MappingItem> ItemClearCommand { get; private set; }
		public DelegateCommand FilePickCommand { get; private set; }
		public DelegateCommand CreateMappingCommand { get; private set; }
		public DelegateCommand<CatalogBase> CatalogChangedCommand { get; private set; }

		#region private members
		private void RaiseFilePickInteractionRequest()
		{
			var itemVM = Container.Resolve<IPickAssetViewModel>();
			itemVM.InitializeViewForSingletonViewModel();

			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(itemVM.Validate) { Content = itemVM, Title = "Select file" },
				(x) =>
				{
					if (x.Confirmed)
					{
						InnerItem.TemplateId = itemVM.SelectedAsset.FolderItemId;
						CsvFileColumns = GetCsvColumns(InnerItem.TemplateId);
						OnPropertyChanged("InnerItem");
					}
				});
		}

		private string[] GetCsvColumns(string filePath)
		{
			string[] retVal = null;

			
			try
			{
				retVal = _importService.GetCsvColumns(filePath, InnerItem.ColumnDelimiter);
			}
			catch(Exception)
			{

			}
			return retVal;
		}

		private void RaiseCreateMappingInteractionRequest()
		{
			var newMapping = new ColumnMapping();
			newMapping.EntityImporter = InnerItem.EntityImporter;

			var itemVM = Container.Resolve<IColumnMappingEditViewModel>(
				new ParameterOverride("item", newMapping)
				);
			
			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(itemVM.Validate) { Content = itemVM, Title = "Create mapping" },
				(x) =>
				{
					if (x.Confirmed)
					{
						if (AllAvailableColumnMappings != null)
						{
							AllAvailableColumnMappings.Add(newMapping);
						}
						else
						{
							AllAvailableColumnMappings = new ObservableCollection<ColumnMapping>() { newMapping };
						}
						OnPropertyChanged("AllAvailableColumnMappings");

						InnerItem.ColumnMapping = newMapping;
						InnerItem.ColumnMappingId = newMapping.ColumnMappingId;
						OnPropertyChanged("InnerItem");
					}
				});
		}

		private void RaiseItemEditInteractionRequest(MappingItem originalItem)
		{
			var item = originalItem.DeepClone<MappingItem>(_entityFactory as CatalogEntityFactory);
			var selectedProperty = SelectedEntityImporter.SystemProperties.First(prop => prop.Name == item.EntityColumnName);
			var param = new ColumnMappingEntity { MappingItem = item, CsvColumnsList = CsvFileColumns, ImportProperty = selectedProperty };
						
			var itemVM = Container.Resolve<IColumnMappingViewModel>(
				new ParameterOverride("item", param)
				);

			var confirmation = new ConditionalConfirmation(itemVM.Validate);
			confirmation.Title = "Edit column mapping";
			confirmation.Content = itemVM;

			CommonConfirmRequest2.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					// copy all values to original:
					if (!String.IsNullOrEmpty(item.CustomValue) && (item.CsvColumnName == "Custom" || String.IsNullOrEmpty(item.CsvColumnName)))
					{
						originalItem.CustomValue = item.CustomValue;
						originalItem.CsvColumnName = null; // String.Empty;
					}
					else
					{
						originalItem.CsvColumnName = item.CsvColumnName;
						originalItem.CustomValue = null; // String.Empty;
					}
					// fake assign for UI triggers to display updated values.
					//originalItem.MappingItemId = item.MappingItemId;
					//SelectedColumnMapping = InnerItem.ColumnMapping;

					if (IsSingleDialogEditing)
					{
						IsModified = true;
					}
				}
			});
		}

		private void RaiseItemClearInteractionRequest(MappingItem originalItem)
		{
			originalItem.CsvColumnName = String.Empty;
			originalItem.CustomValue = String.Empty;
			OnPropertyChanged("InnerItem");
			if (IsSingleDialogEditing)
				IsModified = true;
		}
		
		private void OnCatalogChangesCommand(CatalogBase selectedItem)
		{
			AllAvailablePropertySets = Container.Resolve<ICatalogRepository>().PropertySets.Where(x => x.CatalogId == selectedItem.CatalogId).ToArray();
			OnPropertyChanged("AllAvailablePropertySets");
		}

		private void OnPropertySetChangesCommand(PropertySet x)
		{
			if (!IsSingleDialogEditing)
			{
				OnIsValidChanged();
			}
			else
			{
				IsModified = true;
			}
			//TODO update custom properties map
		}

		private void OnImporterChangesCommand(EntityImporterBase x)
		{
			SelectedEntityImporter = x;
			if (!IsSingleDialogEditing)
			{
				OnIsValidChanged();
			}
			else
			{
				IsModified = true;
			}
		}
		#endregion

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest2 { get; private set; }

        /// <summary>
        /// determines editing state. If IsSingleDialogEditing == true editing in separate dialog. Otherwise editing inside a wizard.
        /// </summary>
        public bool IsSingleDialogEditing { get; private set; }

		private EntityImporterBase _selectedEntityImporter;
		public virtual EntityImporterBase SelectedEntityImporter
		{
			get
			{
				return _selectedEntityImporter;
			}
			set
			{
				_selectedEntityImporter = value;
				OnUIThread(() =>
				{
					if (AllAvailableColumnMappings != null)
						AllAvailableColumnMappings.Clear();
					else
						AllAvailableColumnMappings = new ObservableCollection<ColumnMapping>();
					if (value != null)
						_itemRepository.ColumnMappings.ExpandAll().Where(x => x.EntityImporter == value.Name).ToList().ForEach(y => AllAvailableColumnMappings.Add(y));
				});

				OnPropertyChanged("AllAvailableColumnMappings");
				OnPropertyChanged("SelectedEntityImporter");
			}
		}

        #region ViewModelBase Members
        public override string IconSource
        {
            get
            {
                return "importJob";
            }
        }
        public override string DisplayName
        {
            get
            {
                return _originalItem.Name;
            }
        }

        #endregion

        #region IImportJobViewModel Members
        private ImportJob _innerItem;
        public ImportJob InnerItem
        {
            get { return _innerItem; }
            set
            {
                if (_innerItem != null)
                {
                    DetachEventListeners();
                }

                _innerItem = value;
                OnPropertyChanged("InnerItem");
            }
        }

		public void RaiseCanExecuteChanged()
		{
			ItemEditCommand.RaiseCanExecuteChanged();
			ItemClearCommand.RaiseCanExecuteChanged();
		}

        private MappingItem[] _selectedMappingItems;
        public virtual MappingItem[] SelectedMappingItems
        {
            get { return _selectedMappingItems; }
            set
            {
                _selectedMappingItems = value;
                OnPropertyChanged("SelectedMappingItems");
            }
        }

        public ColumnMapping SelectedColumnMapping
        {
            set
            {	
                var mapping = value;
                MappingItem[] result = null;
                if (mapping != null)
                {					
                    var sys = mapping.SystemPropertiesMap;
                    var cst = mapping.CustomPropertiesMap;
                    if (sys != null)
                    {
                        if (cst != null)
                            result = sys.Concat(cst).ToArray();
                        else
                            result = sys.ToArray();
                    }
					else if (cst != null)
					{
						result = cst.ToArray();
					}
					else
					{
						
						var tmp = AllAvailableEntityImporters.FirstOrDefault(x => x.Name == InnerItem.EntityImporter);
						if (tmp != null)
						{
							var newList = new List<MappingItem>();
							tmp.SystemPropertyNames.ToList().ForEach(y => newList.Add(new MappingItem { EntityColumnName = y }));
							result = newList.ToArray();
							mapping.SystemPropertiesMap = new ObservableCollection<MappingItem>(result);
						}
					}
                }
								
                SelectedMappingItems = result;
				

				if (!IsSingleDialogEditing)
				{
					OnIsValidChanged();
				}
				else
				{
					//IsModified = true;
				}

            }
        }

        public void Delete(IImportRepository repository)
        {
            repository.Attach(_originalItem);
            repository.Remove(_originalItem);
            //repository.DeleteImportJob(InnerItem.Id);
        }
        #endregion

        #region IClosable Members

        public event EventHandler CloseViewRequestedEvent;
        public NavigationItem NavigationData { get; protected set; }

        protected void OnCloseViewRequestedEvent(EventArgs args)
        {
            EventHandler handler = CloseViewRequestedEvent;
            if (handler != null)
                handler(this, args);
        }

        #endregion

        #region ISupportAcceptChanges Members

        public InteractionRequest<Confirmation> CancelConfirmRequest { get; private set; }
        public DelegateCommand<object> CancelCommand { get; private set; }
        public DelegateCommand<object> SaveChangesCommand { get; private set; }

        private bool _isModified;
        public bool IsModified
        {
            get
            {
                return _isModified;
            }

            set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    OnPropertyChanged("IsModified");
                    SaveChangesCommand.RaiseCanExecuteChanged();
                }
            }
        }

        protected void OnCancelCommand(object arg)
        {
            if (IsModified)
            {
                CancelConfirmRequest.Raise(new RefusedConfirmation { Content = "Save changes to Import job '" + DisplayName + "'?", Title = "Action confirmation" },
                    (x) =>
                    {
                        if (x.Confirmed)
                        {
                            //if (InnerItem.Validate())
                            //{
                            IsInitializing = true;

                            PerformTasksInBackgroundWorker((Action)delegate()
                            {
                                DoSaveChanges();
                                OnUIThread((Action)delegate() { OnCloseViewRequestedEvent(EventArgs.Empty); });
                            }, (Action)delegate() { IsInitializing = false; });
                            //}
                        }
                        else if (((RefusedConfirmation)x).Refused)
                        {
                            InnerItem = _originalItem;

                            IsModified = false;
                            OnCloseViewRequestedEvent(EventArgs.Empty);
                        }
                        else
                        {
                            // cancel closing
                            var cancelArgs = arg as System.ComponentModel.CancelEventArgs;
                            if (cancelArgs != null)
                            {
                                cancelArgs.Cancel = true;
                                OpenItemCommand.Execute();
                            }
                        }
                    });
            }
            else
            {
                OnCloseViewRequestedEvent(EventArgs.Empty);
            }
        }

        protected void OnSaveChangesCommand()
        {
            //if (InnerItem.Validate())
            //{
            IsInitializing = true;

            PerformTasksInBackgroundWorker((Action)delegate()
            {
                DoSaveChanges();
                InitializeForOpen();
            }, (Action)delegate() { IsInitializing = false; });
            //}
        }

        private void DoSaveChanges()
        {
			if (InnerItem.EntityImporter != SelectedEntityImporter.Name)
			{
				InnerItem.EntityImporter = SelectedEntityImporter.Name;
			}

			_itemRepository.UnitOfWork.Commit();
            //ItemRepository.SaveImportJob(InnerItem);

            _originalItem = InnerItem;

            OnPropertyChanged("DisplayName");
			if (IsSingleDialogEditing)
				IsModified = false;
        }

        #endregion

        #region IMinimizable Members
        // event action for this EventHandler to be assigned in controller.
        public event EventHandler MinimizableViewRequestedEvent = delegate { };

        public DelegateCommand MinimizeCommand
        {
            get;
            protected set;
        }

        #endregion

        #region IOpenTracking Members

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged("IsActive"); }
        }

        public DelegateCommand OpenItemCommand
        {
            get;
            protected set;
        }

        #endregion

        #region IWizardStep Members

        public override bool IsValid
        {
            get
            {
                // impl. in derived (wizard) classes
                throw new NotImplementedException();
            }
        }

        public override bool IsLast
        {
            get
            {
                return this is IImportJobMappingStepViewModel;
            }
        }

        public override string Description
        {
            get
            {
                // impl. in derived (wizard) classes
                throw new NotImplementedException();
            }
        }
        #endregion

        #region ISupportDelayInitialization Members

        public void InitializeForOpen()
        {
            // initialize if needed
            if (InnerItem == _originalItem)
            {
                IsInitializing = true;

                DetachEventListeners();

                
                // Load complete item here
                _originalItem = LoadItem(_originalItem.ImportJobId);
                InnerItem = _originalItem.DeepClone(_entityFactory as IKnownSerializationTypes);
                // current workaround for repository crash
                //ItemRepository = Container.Resolve<IImportRepository>();
				_itemRepository.Attach(InnerItem);

                InitializePropertiesForViewing();

				if (InnerItem.EntityImporter != null)
					SelectedEntityImporter = AllAvailableEntityImporters.First(x => x.Name == InnerItem.EntityImporter);
				else
					SelectedEntityImporter = AllAvailableEntityImporters.First();

				CsvFileColumns = GetCsvColumns(InnerItem.TemplateId);                

				InnerItem.PropertyChanged += CurrentItem_PropertyChanged;
				 
                //OnUIThread(() =>
                //{

                //});
            }
        }

        private void InitializePropertiesForViewing()
        {
            if (this is IImportJobOverviewStepViewModel || IsSingleDialogEditing)
            {
                if (AllAvailableEntityImporters == null)
                {
					AllAvailableEntityImporters = new EntityImporterBase[] 
					{ 
						new ItemImporter("Product"), 
						new ItemImporter("Sku"), 
						new ItemImporter("Bundle"), 
						new ItemImporter("Package"), 
						new ItemImporter("DynamicKit"), 
						new CategoryImporter(), 
						new AssociationImporter(), 
						new PriceImporter() 
					};

					if (InnerItem.EntityImporter != null)
						SelectedEntityImporter = AllAvailableEntityImporters.FirstOrDefault(x => x.Name == InnerItem.EntityImporter);
					else
						SelectedEntityImporter = AllAvailableEntityImporters.First();

                    OnPropertyChanged("AllAvailableEntityImporters");

                    AllAvailableColumnDelimiters = GetColumnDelimiters();
                    OnPropertyChanged("AllAvailableColumnDelimiters");
                }

				if (AllAvailableCatalogs == null)
				{
					AllAvailableCatalogs = Container.Resolve<ICatalogRepository>().Catalogs.ToArray();
					OnPropertyChanged("AllAvailableCatalogs");
				}

				if (AllAvailablePropertySets == null)
				{
					AllAvailablePropertySets = Container.Resolve<ICatalogRepository>().PropertySets.ToArray(); //.Where(x => x.CatalogId == InnerItem.ContainerId && x.TargetType == InnerItem.EntityImporter).ToArray();
					OnPropertyChanged("AllAvailablePropertySets");
				}

                //if (AllAvailableCurrencies == null)
                //{
                //    AllAvailableCurrencies = ImportJobViewModel.GetAvailableCurrencies();
                //    OnPropertyChanged("AllAvailableCurrencies");
                //}
            }

            if (this is IImportJobMappingStepViewModel || IsSingleDialogEditing)
            {
            }
        }

        #endregion
		
        #region private members

        private ImportJob LoadItem(string itemId)
        {
			return _itemRepository.ImportJobs.Expand("ColumnMapping/SystemPropertiesMap,ColumnMapping/CustomPropertiesMap").Where(x => x.ImportJobId == itemId).Single();
        }

        private void CurrentItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
			if (IsSingleDialogEditing)
			{
				IsModified = true;
			}
			else
			{
				OnIsValidChanged();
			}
        }


        private void DetachEventListeners()
        {
            InnerItem.PropertyChanged -= CurrentItem_PropertyChanged;
        }

        private T CreateEntity<T>()
        {
            return (T)_entityFactory.CreateEntityForType(_entityFactory.GetEntityTypeStringName(typeof(T)));
        }

        private ColumnDelimiter[] GetColumnDelimiters()
        {
            return new ColumnDelimiter[] { 
                new ColumnDelimiter{ DisplayName = "Comma", Value=',' },
                new ColumnDelimiter{ DisplayName = "Semicolon", Value=';' },
                new ColumnDelimiter{ DisplayName = "Tab", Value='\t' },
				new ColumnDelimiter{ DisplayName = "Other", Value=' ' }
            };
        }

		private TextQualifier[] GetTextQualifiers()
		{
			return new TextQualifier[] { 
                new TextQualifier{ DisplayName = "Quote", Value='\'' },
				new TextQualifier{ DisplayName = "Other", Value=' ' }
            };
		}

        #endregion
    }
}
