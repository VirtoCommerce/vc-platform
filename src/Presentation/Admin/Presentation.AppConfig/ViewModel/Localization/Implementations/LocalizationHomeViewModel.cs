using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Win32;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Csv;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Model;
using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Implementations
{
	public class LocalizationHomeViewModel : HomeSettingsViewModel<LocalizationGroup>, ILocalizationHomeViewModel, IEntityExportable
	{
		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;
		private readonly IViewModelsFactory<ILocalizationEditViewModel> _editVmFactory;
		private readonly IElementRepository _elementRepository;

		#endregion

		#region Constructor
		public LocalizationHomeViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory,
			IViewModelsFactory<ILocalizationEditViewModel> editVmFactory, IElementRepository elementRepository)
			: base(entityFactory)
		{
			_editVmFactory = editVmFactory;
			_repositoryFactory = repositoryFactory;
			_elementRepository = elementRepository;
			InitCommands();
		}

		#endregion

		#region Commands init

		private void InitCommands()
		{
			ItemEditCommand = new DelegateCommand<LocalizationGroup>(RaiseItemEditInteractionRequest, CanRaiseItemEditExecute);
			ClearFiltersCommand = new DelegateCommand(DoClearFilters);
			SearchItemsCommand = new DelegateCommand(DoSearchItems);
			ListExportCommand = new DelegateCommand(RaiseExportCommand, CanExecuteExport);
			ClearCacheCommand = new DelegateCommand(DoClearCache);

			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			CommonNotifyRequest = new InteractionRequest<Notification>();
		}

		#endregion

		#region Filters

		public string SearchFilterName { get; set; }
		public string FilterModule { get; set; }
		public DelegateCommand ClearFiltersCommand { get; private set; }
		public DelegateCommand SearchItemsCommand { get; private set; }

		private int dataLoadingworker;
		private readonly object _lockObject = new object();

		private void DoClearFilters()
		{
			SearchFilterName = null;
			FilterModule = null;
			IsUntranslatedOnly = false;
			OriginalLanguage = null;
			TranslateLanguage = null;
			OnPropertyChanged("SearchFilterName");
			OnPropertyChanged("FilterModule");
			RaiseCanExecuteChanged();
		}

		private void DoSearchItems()
		{
			if (RefreshItemListCommand != null)
				RefreshItemListCommand.Execute();
		}

		private void DoClearCache()
		{
			var confirmation = new ConditionalConfirmation();
			confirmation.Title = "Refresh locally cached Commerce Manager texts".Localize();
			confirmation.Content = "Are you sure you want to clear all locally cached Commerce Manager texts?".Localize();

			CommonConfirmRequest.Raise(confirmation,
				async x =>
				{
					if (x.Confirmed)
					{
						ShowLoadingAnimation = true;
						try
						{
							await Task.Run(() =>
								{
									_elementRepository.Clear();

									// force Elements re-caching
									_elementRepository.Elements();
								});

							var notification = new Notification();
							// notification.Title = "Done";
							notification.Content = "All locally cached texts were removed. You may need to restart this application for the changes to take effect."
								.Localize();
							CommonNotifyRequest.Raise(notification);
						}
						finally
						{
							ShowLoadingAnimation = false;
						}
					}
				});
		}

		#endregion

		#region ILocalizationHomeViewModel Members

		public ICollectionView ListItemsSource { get; private set; }

		#endregion

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			var items = new List<LocalizationGroup>();

			if (dataLoadingworker == 0 &&
				(LanguagesCodes == null ||
					(LanguagesCodes.Contains(OriginalLanguage) &&
					 LanguagesCodes.Contains(TranslateLanguage))))
			{
				lock (_lockObject)
				{
					if (dataLoadingworker == 0)
					{
						dataLoadingworker = System.Threading.Thread.CurrentThread.ManagedThreadId;
					}
				}

				if (dataLoadingworker == System.Threading.Thread.CurrentThread.ManagedThreadId)
				{
					try
					{
						using (var repository = _repositoryFactory.GetRepositoryInstance())
						{
							if (LanguagesCodes == null)
							{
								// load Languages setting
								var langSetting =
									repository.Settings.Expand(s => s.SettingValues).Where(s => s.Name.Contains("Lang")).SingleOrDefault();

								OnUIThread(() =>
									{
										LanguagesCodes = langSetting == null
															 ? new List<string>()
															 : langSetting.SettingValues.Select(sv => sv.ShortTextValue).ToList();

										// list all module names
										FilterModules = new[]
											{
												new KeyValuePair_string_string {Value = "Web pages"},
												new KeyValuePair_string_string {Key = LocalizationScope.DefaultCategory, Value = "General"},
												new KeyValuePair_string_string {Key = "AppConfig", Value = "AppConfig"},
												new KeyValuePair_string_string {Key = "Asset", Value = "Asset"},
												new KeyValuePair_string_string {Key = "Catalog", Value = "Catalog"},
												new KeyValuePair_string_string {Key = "Configuration", Value = "Configuration"},
												new KeyValuePair_string_string {Key = "Customers", Value = "Customers"},
												new KeyValuePair_string_string {Key = "DynamicContent", Value = "DynamicContent"},
												new KeyValuePair_string_string {Key = "Fulfillment", Value = "Fulfillment"},
												new KeyValuePair_string_string {Key = "Import", Value = "Import"},
												new KeyValuePair_string_string {Key = "Main", Value = "Main"},
												new KeyValuePair_string_string {Key = "Marketing", Value = "Marketing"},
												new KeyValuePair_string_string {Key = "Order", Value = "Order"},
												new KeyValuePair_string_string {Key = "Reporting", Value = "Reporting"},
												new KeyValuePair_string_string {Key = "Reviews", Value = "Reviews"},
												new KeyValuePair_string_string {Key = "Security", Value = "Security"}
											}.ToList();
									});
							}

							if (LanguagesCodes.Contains(OriginalLanguage) &&
								LanguagesCodes.Contains(TranslateLanguage))
							{
								var query = repository.Localizations;

								if (string.IsNullOrEmpty(FilterModule))
								{
									query = query.Where(x => x.Category == "");
								}
								else
								{
									query = query.Where(x => x.Category == FilterModule);
								}

								var names = query.ToList().Select(x => x.Name).Distinct();
								var translateItems = query.Where(x => x.LanguageCode == TranslateLanguage).ToDictionary(x => x.Name);
								var originalItems = query.Where(x => x.LanguageCode == OriginalLanguage).ToDictionary(x => x.Name);

								foreach (var name in names)
								{
									var item = name;
									var isTranslateExists = translateItems.ContainsKey(name);
									if ((IsUntranslatedOnly && !isTranslateExists) || !IsUntranslatedOnly)
									{
										var original = originalItems.ContainsKey(name)
														   ? originalItems[name]
														   : new Foundation.AppConfig.Model.Localization();
										var translated = isTranslateExists
															 ? translateItems[name]
															 : new Foundation.AppConfig.Model.Localization()
																 {
																	 Name = original.Name,
																	 LanguageCode = TranslateLanguage,
																	 Category = original.Category
																 };
										if (string.IsNullOrEmpty(SearchFilterName)
											|| item.Contains(SearchFilterName)
											||
											(!string.IsNullOrEmpty(original.Value) &&
											 original.Value.IndexOf(SearchFilterName, StringComparison.OrdinalIgnoreCase) >= 0)
											||
											(!string.IsNullOrEmpty(translated.Value) &&
											 translated.Value.IndexOf(SearchFilterName, StringComparison.OrdinalIgnoreCase) >= 0))
										{
											items.Add(new LocalizationGroup()
												{
													Name = item,
													OriginalLocalization = original,
													TranslateLocalization = translated
												});
										}
									}
								}
							}
						}
					}
					finally
					{
						dataLoadingworker = 0;
					}
				}
			}

			return items;
		}

		public override void RefreshItem(object item)
		{
			//var itemToUpdate = item as Foundation.AppConfig.Model.Localization;
			//if (itemToUpdate != null)
			//{
			//	Foundation.AppConfig.Model.Localization itemFromInnerItem =
			//		Items.SingleOrDefault(et => et.EmailTemplateId == itemToUpdate.EmailTemplateId);

			//	if (itemFromInnerItem != null)
			//	{
			//		OnUIThread(() =>
			//		{
			//			itemFromInnerItem.InjectFrom<CloneInjection>(itemToUpdate);
			//			OnPropertyChanged("Items");
			//		});
			//	}
			//}
		}

		public override void RaiseCanExecuteChanged()
		{
			ItemEditCommand.RaiseCanExecuteChanged();
			ListExportCommand.RaiseCanExecuteChanged();
		}

		#endregion

		#region Public members

		public List<KeyValuePair_string_string> FilterModules
		{
			get { return _filterModules; }
			set { _filterModules = value; OnPropertyChanged(); }
		}

		private bool _isUntranslatedOnly;
		public bool IsUntranslatedOnly
		{
			get { return _isUntranslatedOnly; }
			set
			{
				_isUntranslatedOnly = value;
				OnPropertyChanged();
			}
		}


		private string _originalLanguage;
		public string OriginalLanguage
		{
			get { return _originalLanguage; }
			set
			{
				_originalLanguage = value;
				OnPropertyChanged();
				OnPropertyChanged("OriginalLangName");
			}
		}

		private string _translateLanguage;
		public string TranslateLanguage
		{
			get { return _translateLanguage; }
			set
			{
				_translateLanguage = value;
				OnPropertyChanged();
				OnPropertyChanged("TranslateLangName");
			}
		}

		private List<string> _languagesCodes;
		private List<KeyValuePair_string_string> _filterModules;

		public List<string> LanguagesCodes
		{
			get { return _languagesCodes; }
			set
			{
				_languagesCodes = value;
				OnPropertyChanged();
			}
		}

		public string OriginalLangName
		{
			get
			{
				if (string.IsNullOrEmpty(OriginalLanguage) || OriginalLanguage.Length > 5)
				{
					return OriginalLanguage;
				}
				return string.Format("{0} ({1})",
					CultureInfo.GetCultureInfo(OriginalLanguage).DisplayName,
					OriginalLanguage);
			}
		}

		public string TranslateLangName
		{
			get
			{
				if (string.IsNullOrEmpty(TranslateLanguage) || TranslateLanguage.Length > 5)
				{
					return TranslateLanguage;
				}
				return string.Format("{0} ({1})",
					CultureInfo.GetCultureInfo(TranslateLanguage).DisplayName,
					TranslateLanguage);
			}
		}

		#endregion

		#region Commands

		public DelegateCommand<LocalizationGroup> ItemEditCommand { get; private set; }
		public DelegateCommand ListExportCommand { get; private set; }
		public DelegateCommand ClearCacheCommand { get; private set; }

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }

		private void RaiseItemEditInteractionRequest(LocalizationGroup item)
		{
			var itemVM = _editVmFactory.GetViewModelInstance(
				 new System.Collections.Generic.KeyValuePair<string, object>("item", item),
				 new System.Collections.Generic.KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		private static bool CanRaiseItemEditExecute(LocalizationGroup item)
		{
			return item != null && item.OriginalLocalization != null && !string.IsNullOrEmpty(item.OriginalLocalization.Value) &&
				   !string.IsNullOrEmpty(item.TranslateLocalization.LanguageCode);
		}

		private bool CanExecuteExport()
		{
			return Items != null && Items.Any();
		}


		#endregion

		#region IEntityExportable Members

		private void RaiseExportCommand()
		{
			var filePath = ShowSaveDialog();

			if (!string.IsNullOrEmpty(filePath))
			{
				Task.Run(() =>
					{
						var id = Guid.NewGuid().ToString();

						var statusUpdate = new StatusMessage
							{
								ShortText = string.Format("Localization export to '{0}'.", filePath),
								StatusMessageId = id
							};
						EventSystem.Publish(statusUpdate);

						PerformExportAsync(id, filePath);

					});
			}
		}

		#endregion

		#region Auxilliary methods

		private void PerformExportAsync(string id, string filePath)
		{
			try
			{
				using (var textWriter = File.CreateText(filePath))
				{
					var csvWriter = new CsvWriter(textWriter, ",");
					csvWriter.WriteRow(new List<string> { "Name", OriginalLangName, string.Format("LanguageCode ({0})", TranslateLanguage), string.Format("Value - {0}", TranslateLangName) },
									   false);
					var itemsCount = Items.Count();
					foreach (var item in Items.Select((value, index) => new { value, index }))
					{
						csvWriter.WriteRow(item.value.ToExportCollection(), true);
						var statusUpdate = new StatusMessage
							{
								Details = string.Format("Exported {0} of {1}.", item.index, itemsCount),
								StatusMessageId = id
							};
						EventSystem.Publish(statusUpdate);

					}
				}

				var finalStatus = new StatusMessage
				{
					ShortText = string.Format("Localization export to '{0}' finished successfully.", filePath),
					StatusMessageId = id,
					State = StatusMessageState.Success
				};
				EventSystem.Publish(finalStatus);
			}
			catch (Exception ex)
			{
				var finalStatus = new StatusMessage
				{
					ShortText = string.Format("Error occured during localization export to '{0}'.", filePath),
					StatusMessageId = id,
					State = StatusMessageState.Error,
					Details = ex.Message
				};
				EventSystem.Publish(finalStatus);
			}
		}

		private string ShowSaveDialog()
		{
			var dialog = new SaveFileDialog()
			{
				FileName = string.Format("From {0} to {1}", OriginalLanguage, TranslateLanguage),
				Filter = "Comma separated Files(*.csv)|*.csv|All(*.*)|*"
			};

			string retVal = null;

			if (dialog.ShowDialog() == true && !string.IsNullOrEmpty(dialog.FileName))
			{
				retVal = dialog.FileName;
				if (string.IsNullOrEmpty(Path.GetExtension(retVal)))
					retVal = string.Format("{0}.csv", retVal);
			}

			return retVal;
		}
		#endregion

	}
}
