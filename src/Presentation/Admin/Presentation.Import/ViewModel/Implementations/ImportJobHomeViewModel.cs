using VirtoCommerce.Client.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Importing.Factories;
using VirtoCommerce.Foundation.Importing.Services;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Import.Model;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Import.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Foundation.Importing.Repositories;
using System;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Implementations
{
	public class ImportJobHomeViewModel : ViewModelHomeEditableBase<ImportJob>, IImportJobHomeViewModel, IVirtualListLoader<IImportJobViewModel>, ISupportDelayInitialization
	{
		#region Dependencies

		private readonly IImportJobEntityFactory _entityFactory;
		private readonly IRepositoryFactory<IImportRepository> _importRepository;
		private readonly IViewModelsFactory<ICreateImportJobViewModel> _wizardVmFactory;
		private readonly IViewModelsFactory<IImportJobRunViewModel> _runVmFactory;
		private readonly IViewModelsFactory<IImportJobViewModel> _itemVmFactory;
		private readonly IAuthenticationContext _authContext;
		private readonly IImportService _importService;
		private readonly SubTabsDefaultViewModel _parentViewModel;
		
		#endregion
		
		public string HomeMenuName { get; set; }

		#region Constructor
		public ImportJobHomeViewModel(
			IRepositoryFactory<IImportRepository> importRepository,
			IViewModelsFactory<ICreateImportJobViewModel> wizardVmFactory,
			IViewModelsFactory<IImportJobRunViewModel> runVmFactory,
			IViewModelsFactory<IImportJobViewModel> itemVmFactory,
			IImportJobEntityFactory entityFactory, 
			IAuthenticationContext authContext,
			IImportService importService,
			SubTabsDefaultViewModel parentViewModel
			)
		{
			_entityFactory = entityFactory;
			_importRepository = importRepository;
			_wizardVmFactory = wizardVmFactory;
			_runVmFactory = runVmFactory;
			_itemVmFactory = itemVmFactory;
			_authContext = authContext;
			_parentViewModel = parentViewModel;
			_importService = importService;

			AvailableImporters = (ImportEntityType[]) Enum.GetValues(typeof(ImportEntityType));

			InitCommands();
            ViewTitle = new ViewTitleBase() { Title = "Catalogs", SubTitle = "MERCHANDISE MANAGEMENT".Localize() };
		}

		#region ViewModelHomeEditableBase

		protected override bool CanItemAddExecute()
		{
			return _authContext.CheckPermission(PredefinedPermissions.CatalogCatalog_Import_JobsManage);
		}

		protected override bool CanItemDeleteExecute(IList x)
		{
			return _authContext.CheckPermission(PredefinedPermissions.CatalogCatalog_Import_JobsManage) && x != null &&
				   x.Count > 0;
		}

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = _entityFactory.CreateEntity<ImportJob>();
			item.ColumnDelimiter = "?";
			item.EntityImporter = DefaultImporter;
			var itemVM = _wizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("entityImporters", AvailableImporters)
				);

			var confirmation = new Confirmation { Content = itemVM, Title = "Create Import Job definition".Localize() };
			
			ItemAdd(confirmation);
		}

		protected override void RaiseItemDeleteInteractionRequest(IList selectedItemsList)
		{
			var selectedItems = selectedItemsList.Cast<VirtualListItem<IImportJobViewModel>>();
			ItemDelete(selectedItems.Select(x => ((IViewModelDetailBase)x.Data)).ToList());
		}

		#endregion
		
		private void InitCommands()
		{
			CommonNotifyRequest = new InteractionRequest<Notification>();
			ImportJobRunRequest = new InteractionRequest<Confirmation>();

			ClearFiltersCommand = new DelegateCommand(DoClearFilters);

			ImportJobRunCommand = new DelegateCommand<object>(RaiseImportJobRunInteractionRequest, x => _authContext.CheckPermission(PredefinedPermissions.CatalogCatalog_Import_JobsRun) && x != null);
			ItemDuplicateCommand = new DelegateCommand<IList>(RaiseItemDuplicateInteractionRequest, x => _authContext.CheckPermission(PredefinedPermissions.CatalogCatalog_Import_JobsManage) && x != null);
		}
		#endregion

		#region Filters
		public string SearchFilterKeyword { get; set; }
		public string SearchFilterName { get; set; }
		public DelegateCommand ClearFiltersCommand { get; private set; }
		#endregion

		#region IImportJobHomeViewModel Members

		public InteractionRequest<Confirmation> ImportJobRunRequest { get; private set; }
		public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }

		public DelegateCommand<object> ImportJobRunCommand { get; private set; }
		public DelegateCommand<IList> ItemDuplicateCommand { get; private set; }
		

		public string DefaultImporter { get; set; }
		public ImportEntityType[] AvailableImporters { get; set; }
		#endregion

		#region IVirtualListLoader<IImportJobViewModel> Members
		public bool CanSort
		{
			get { return true; }
		}

		public IList<IImportJobViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<IImportJobViewModel>();
			using (var repository = _importRepository.GetRepositoryInstance())
			{
				var query = repository.ImportJobs;

				if (!string.IsNullOrEmpty(SearchFilterKeyword))
					query = query.Where(x => x.Name.Contains(SearchFilterKeyword));
				else
				{
					if (!string.IsNullOrEmpty(SearchFilterName))
						query = query.Where(x => x.Name.Contains(SearchFilterName));
				}

				overallCount = query.Count();
				var orderedItems = !sortDescriptions.Any() ? query.OrderBy(x => x.Name) : ApplySortDescriptions(query, sortDescriptions);
				var items = orderedItems.Skip(startIndex).Take(count).ToList();
				foreach (var item in items)
				{
					if (AvailableImporters.Any(importer => importer.ToString() == item.EntityImporter))
					{
						var itemViewModel = _itemVmFactory.GetViewModelInstance(
							new KeyValuePair<string, object>("item", item),
							new KeyValuePair<string, object>("entityImporters", AvailableImporters),
							new KeyValuePair<string, object>("homeMenuName", _parentViewModel.CurrentTab.IdTab)
							);
						retVal.Add(itemViewModel);
					}
					else
					{
						overallCount -= 1;
					}
				}
			}
			return retVal;
		}
		#endregion

		#region ISupportDelayInitialization Members

		public void InitializeForOpen()
		{
			if (ListItemsSource == null)
			{
				OnUIThread(()=>ListItemsSource = new VirtualList<IImportJobViewModel>(this, 25, SynchronizationContext.Current));
			}
			OnUIThread(InitializeGestures);
		}

		#endregion

		#region private members
		
		private void RaiseImportJobRunInteractionRequest(object item)
		{
			// initial checks
			if (item == null)
			{
				CommonNotifyRequest.Raise(new Notification { Content = "Select import job to run.".Localize(), Title = "Error".Localize(null, LocalizationScope.DefaultCategory) });
			}
			else
			{
				var it = ((VirtualListItem<IImportJobViewModel>) item).Data.InnerItem;
				var jobEntity = new ImportEntity { ImportJob = it };
				var itemVM = _runVmFactory.GetViewModelInstance(
					new KeyValuePair<string, object>("jobEntity", jobEntity)
					);

				var confirmation = new ConditionalConfirmation(itemVM.Validate) { Content = itemVM, Title = "Run Import Job".Localize() };
				CommonConfirmRequest.Raise(confirmation, async (x) =>
				{
					if (x.Confirmed)
					{
						await Task.Run(() =>
							{
								var id = Guid.NewGuid().ToString();

								var statusUpdate = new StatusMessage
									{
										ShortText = string.Format("File '{0}' import.".Localize(), Path.GetFileName(jobEntity.SourceFile)),
										StatusMessageId = id
									};
								EventSystem.Publish(statusUpdate);

								var progress = new Progress<ImportProgress>();
								progress.ProgressChanged += ImportProgressChanged;
								PerformImportAsync(id, jobEntity, progress);
							});
					}
				});
			}
		}

		private static void ImportProgressChanged(object sender, ImportProgress e)
		{
			if (e != null && e.ImportEntity != null && e.ImportResult != null)
			{
				if (!e.ImportResult.IsFinished)
				{
					//IEnumerable<object> errors = null;
					//if (e.ImportResult.Errors != null)
					//	errors = e.ImportResult.Errors.Cast<object>();
					var statusUpdate = new StatusMessage
						{
							ShortText =
								string.Format("File '{0}' import. Processed {1} items.".Localize(), Path.GetFileName(e.ImportEntity.SourceFile),
											  e.Processed),
							//Details = errors != null ?
							//	errors.Where(val => val != null)
							//	 .Aggregate(string.Empty, (current, val) => current + (val.ToString() + Environment.NewLine)) : string.Empty,
							StatusMessageId = e.StatusId
						};
					EventSystem.Publish(statusUpdate);
				}
				else
				{
					if (e.ImportResult.Errors != null)
					{
						var statusUpdate = new StatusMessage
							{
								ShortText = string.Format("File '{0}' imported with errors".Localize(), Path.GetFileName(e.ImportEntity.SourceFile)),
								StatusMessageId = e.StatusId,
								Details = e.ImportResult.Errors.Cast<object>()
								           .Where(val => val != null)
								           .Aggregate(string.Empty, (current, val) => current + (val.ToString() + Environment.NewLine)),
								State = StatusMessageState.Error
							};
						EventSystem.Publish(statusUpdate);
					}
					else
					{
						var statusUpdate = new StatusMessage
							{
								ShortText = string.Format("File '{0}' imported successfully".Localize(), Path.GetFileName(e.ImportEntity.SourceFile)),
								StatusMessageId = e.StatusId,
								State = StatusMessageState.Success
							};
						EventSystem.Publish(statusUpdate);
					}
				}
			}
		}
		
		
		private async void PerformImportAsync(string id, ImportEntity jobEntity, IProgress<ImportProgress> progress)
		{
			var task = new Task(() => _importService.RunImportJob(jobEntity.ImportJob.ImportJobId, jobEntity.SourceFile));
			task.Start();
			
			if (progress != null)
			{
				var finished = false;
				while (!finished)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(100));
					
					var res = _importService.GetImportResult(jobEntity.ImportJob.ImportJobId);
					progress.Report(new ImportProgress
						{
							ImportEntity = jobEntity,
							ImportResult = res,
							StatusId = id,
							Processed = res == null ? 0 : res.ProcessedRecordsCount + res.ErrorsCount
						});

					if (res != null && res.IsFinished)
						finished = true;
				}
			}
		}
		
		private void RaiseItemDuplicateInteractionRequest(IList selectedItemsList)
		{
			// initial checks
			if (selectedItemsList == null)
			{
				CommonNotifyRequest.Raise(new Notification { Content = "Select import job to duplicate.".Localize(), Title = "Error".Localize(null, LocalizationScope.DefaultCategory) });
			}
			else
			{
				var selectedItems = selectedItemsList.Cast<VirtualListItem<IImportJobViewModel>>();
				ItemDuplicate(selectedItems.Select(x => (IViewModelDetailBase) x.Data).ToList());
			}
		}
		
		private void DoClearFilters()
		{
			SearchFilterKeyword = SearchFilterName = null;
			OnPropertyChanged("SearchFilterKeyword");
			OnPropertyChanged("SearchFilterName");
		}
		
		#endregion

		#region Import progress
		
		private ImportResult _result;
		public ImportResult Result
		{
			get
			{
				return _result;
			}
			set
			{
				_result = value;
				OnPropertyChanged();
				OnPropertyChanged("Processed");
				OnPropertyChanged("Errors");
			}
		}

		public int Processed
		{
			get
			{
				return Result != null ? Result.ProcessedRecordsCount + Result.ErrorsCount : 0;
			}
		}

		public string Errors
		{
			get
			{
				if (Result != null && Result.ErrorsCount > 0)
				{
					return Result.Errors.Cast<object>().Where(val => val != null).Aggregate(string.Empty, (current, val) => current + (val.ToString() + Environment.NewLine));
				}
				return string.Empty;
			}
		}

		#endregion

	}
}
