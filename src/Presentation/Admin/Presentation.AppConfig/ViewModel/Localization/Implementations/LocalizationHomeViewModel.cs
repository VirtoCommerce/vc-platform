using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Implementations
{
	public class LocalizationHomeViewModel : HomeSettingsViewModel<LocalizationGroup>, ILocalizationHomeViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;
		private readonly IViewModelsFactory<ILocalizationEditViewModel> _editVmFactory;

		#endregion

		#region Constructor
		public LocalizationHomeViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, 
			IViewModelsFactory<ILocalizationEditViewModel> editVmFactory)
			: base(entityFactory)
		{
			_editVmFactory = editVmFactory;
			_repositoryFactory = repositoryFactory;
			ItemEditCommand = new DelegateCommand<LocalizationGroup>(RaiseItemEditInteractionRequest, CanRaiseItemEditExecute);
			ClearFiltersCommand = new DelegateCommand(DoClearFilters);
			SearchItemsCommand = new DelegateCommand(DoSearchItems);
		}

		#endregion

		#region Filters

		public string SearchFilterName { get; set; }
		public DelegateCommand ClearFiltersCommand { get; private set; }
		public DelegateCommand SearchItemsCommand { get; private set; }

		private void DoClearFilters()
		{
			SearchFilterName = null;
			IsUntranslatedOnly = false;
			OriginalLanguage = null;
			TranslateLanguage = null;
			OnPropertyChanged("SearchFilterName");
			OnPropertyChanged("IsUntranslatedOnly");
			OnPropertyChanged("OriginalLanguage");
			OnPropertyChanged("TranslateLanguage");
		}

		private void DoSearchItems()
		{
			if (RefreshItemListCommand != null)
				RefreshItemListCommand.Execute();
		}

		#endregion

		#region ILocalizationHomeViewModel Members

		public ICollectionView ListItemsSource { get; private set; }

		#endregion

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			var items = new List<LocalizationGroup>();
			
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					if (!string.IsNullOrEmpty(OriginalLanguage) && OriginalLanguage.Length == 5 &&
						!string.IsNullOrEmpty(TranslateLanguage) && TranslateLanguage.Length == 5)
					{
						var names = repository.Localizations.ToList().Select(x => x.Name).Distinct();
						var translateItems =
							repository.Localizations.Where(x => x.LanguageCode == TranslateLanguage).ToDictionary(x => x.Name);
						var originalItems =
							repository.Localizations.Where(x => x.LanguageCode == OriginalLanguage).ToDictionary(x => x.Name);

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
					// load Language
					var langSetting =
						repository.Settings.Expand(s => s.SettingValues).Where(s => s.Name.Contains("Lang")).SingleOrDefault();

					OnUIThread(() =>
						{
							if ((LanguagesCodes == null || !LanguagesCodes.Any()) && langSetting != null)
								LanguagesCodes = langSetting.SettingValues.Select(sv => sv.ShortTextValue).ToList();
						});
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
		}

		#endregion

		#region Public members


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

		private void RaiseItemEditInteractionRequest(LocalizationGroup item)
		{
			var itemVM = _editVmFactory.GetViewModelInstance(
				 new KeyValuePair<string, object>("item", item),
				 new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		private static bool CanRaiseItemEditExecute(LocalizationGroup item)
		{
			return item != null && item.OriginalLocalization != null && !string.IsNullOrEmpty(item.OriginalLocalization.Value) &&
				   !string.IsNullOrEmpty(item.TranslateLocalization.LanguageCode);
		}


		#endregion

	}
}
