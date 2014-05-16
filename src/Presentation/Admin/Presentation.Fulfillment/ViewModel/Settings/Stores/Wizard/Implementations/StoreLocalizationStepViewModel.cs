using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Stores.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class StoreLocalizationStepViewModel : StoreViewModel, IStoreLocalizationStepViewModel, IMultiSelectControlCommands
	{
		const string SettingName_Currencies = "Currencies";
		const string SettingName_Languages = "Languages";

		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;

		#endregion

		public StoreLocalizationStepViewModel(IStoreEntityFactory entityFactory, Store item,
			IRepositoryFactory<IStoreRepository> repositoryFactory,
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory)
			: base(repositoryFactory, entityFactory, item)
		{
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
		}

		#region Properties

		public ObservableCollection<StoreLanguageDisplay> AllAvailableLanguages { get; private set; }
		public ObservableCollection<StoreLanguageDisplay> InnerItemLanguages { get; private set; }
		public ObservableCollection<StoreCurrency> AllAvailableCurrencies { get; private set; }

		#endregion

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				bool retval;

				bool doNotifyChanges = false;
				// InnerItem.Validate(doNotifyChanges);
				ValidateLanguages(doNotifyChanges);
				ValidateDefaultLanguage(doNotifyChanges);
				ValidateCurrencies(doNotifyChanges);
				ValidateDefaultCurrency(doNotifyChanges);
				retval = InnerItem.Errors.Count == 0;
				InnerItem.Errors.Clear();

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

		//public override string Comment
		//{
		//    get
		//    {
		//        var retVal = String.Empty;
		//        if (this.View is ICreateStoreOverviewStepView)
		//        {
		//            retVal = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
		//        }

		//        return retVal;
		//    }
		//}

		public override string Description
		{
			get
			{
				return "Enter localization information.".Localize();
			}
		}
		#endregion

		#region IMultiSelectControlCommands
		public void SelectItem(object selectedObj)
		{
			if (selectedObj is StoreLanguageDisplay)
			{
				var selectedItem = (StoreLanguageDisplay)selectedObj;
				InnerItem.Languages.Add(selectedItem.Language);
				InnerItemLanguages.Add(selectedItem);
			}
			else if (selectedObj is StoreCurrency)
				InnerItem.Currencies.Add(selectedObj as StoreCurrency);
		}

		public void SelectAllItems(ICollectionView availableItemsCollectionView)
		{
			if (availableItemsCollectionView.SourceCollection is ICollection<StoreLanguageDisplay>)
			{
				var itemsList = availableItemsCollectionView.Cast<StoreLanguageDisplay>().ToList();
				itemsList.ForEach(x => SelectItem(x));
			}
			else if (availableItemsCollectionView.SourceCollection is ICollection<StoreCurrency>)
			{
				var itemsList = availableItemsCollectionView.Cast<StoreCurrency>();
				InnerItem.Currencies.Add(itemsList);
			}
			else if (availableItemsCollectionView.SourceCollection is ICollection<Store>)
			{
				availableItemsCollectionView.Cast<Store>().ToList().ForEach(x => SelectItem(x));
			}
		}

		public void UnSelectItem(object selectedObj)
		{
			StoreLanguageDisplay selectedLanguage;
			StoreCurrency selectedCurrency;

			// prevent removing default language
			if ((selectedLanguage = selectedObj as StoreLanguageDisplay) != null && selectedLanguage.Language.LanguageCode != InnerItem.DefaultLanguage)
			{
				var item = InnerItem.Languages.First(x => x.LanguageCode == selectedLanguage.Language.LanguageCode);
				InnerItem.Languages.Remove(item);
				InnerItemLanguages.Remove(selectedLanguage);
			}
			else if ((selectedCurrency = selectedObj as StoreCurrency) != null && selectedCurrency.CurrencyCode != InnerItem.DefaultCurrency)
			{
				var item = InnerItem.Currencies.First(x => x.CurrencyCode == selectedCurrency.CurrencyCode);
				InnerItem.Currencies.Remove(item);
			}
		}

		public void UnSelectAllItems(System.Collections.IList currentListItems)
		{
			// prevent removing default language
			if (currentListItems is IList<StoreLanguageDisplay>)
			{
				InnerItemLanguages.ToList().ForEach(x => UnSelectItem(x));
			}
			else if (currentListItems is IList<StoreCurrency>)
			{
				InnerItem.Currencies.ToList().ForEach(x => UnSelectItem(x));
			}
		}

		private bool FilterLanguages(object item)
		{
			bool result = true;
			if (item is StoreLanguageDisplay)
			{
				var itemTyped = (StoreLanguageDisplay)item;
				var existLang = InnerItem.Languages.SingleOrDefault(l => l.LanguageCode == itemTyped.Language.LanguageCode);
				if (existLang != null)
				{
					result = false;
				}
			}
			return result;
		}

		private bool FilterCurrencies(object item)
		{
			bool result = true;
			if (item is StoreCurrency)
			{
				var itemTyped = (StoreCurrency)item;
				var existCurrency = InnerItem.Currencies.SingleOrDefault(x => x.CurrencyCode == itemTyped.CurrencyCode);
				if (existCurrency != null)
				{
					result = false;
				}
			}
			return result;
		}

		#endregion

		protected override void InitializePropertiesForViewing()
		{
			if (AllAvailableLanguages == null)
			{
				// all Views must be set on UI thread!!
				using (var appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
				{
					AllAvailableLanguages = GetAvailableLanguages(InnerItem.StoreId, appConfigRepository);
					OnUIThread(() =>
					{
						var defaultView = CollectionViewSource.GetDefaultView(AllAvailableLanguages);
						defaultView.Filter = FilterLanguages;
						defaultView.Refresh();
					});
					OnPropertyChanged("AllAvailableLanguages");

					// currently selected languages
					var InnerItemStoreLanguageDisplays =
						AllAvailableLanguages.Where(
							x =>
								InnerItem.Languages.Any(
									y => y.LanguageCode.Equals(x.Language.LanguageCode, StringComparison.InvariantCultureIgnoreCase)));
					InnerItemLanguages = new ObservableCollection<StoreLanguageDisplay>(InnerItemStoreLanguageDisplays);
					OnPropertyChanged("InnerItemLanguages");

					AllAvailableCurrencies = GetAvailableCurrencies(InnerItem.StoreId, appConfigRepository);

					OnUIThread(() =>
					{
						var defaultView2 = CollectionViewSource.GetDefaultView(AllAvailableCurrencies);
						defaultView2.Filter = FilterCurrencies;
						defaultView2.Refresh();
					});
					OnPropertyChanged("AllAvailableCurrencies");

				}
			}
		}

		private void ValidateLanguages(bool doNotifyChanges)
		{
			if (InnerItem.Languages.Count == 0)
				InnerItem.SetError("Languages", "external validation error".Localize(), doNotifyChanges);
			else
				InnerItem.ClearError("Languages");
		}

		private void ValidateDefaultLanguage(bool doNotifyChanges)
		{
			if (string.IsNullOrEmpty(InnerItem.DefaultLanguage))
				InnerItem.SetError("DefaultLanguage", "Field 'Default Language' is required.".Localize(), doNotifyChanges);
			else
				InnerItem.ClearError("DefaultLanguage");
		}

		private void ValidateCurrencies(bool doNotifyChanges)
		{
			if (InnerItem.Currencies.Count == 0)
				InnerItem.SetError("Currencies", "external validation error".Localize(), doNotifyChanges);
			else
				InnerItem.ClearError("Currencies");
		}

		private void ValidateDefaultCurrency(bool doNotifyChanges)
		{
			if (string.IsNullOrEmpty(InnerItem.DefaultCurrency))
				InnerItem.SetError("DefaultCurrency", "Field 'Default Currency' is required.".Localize(), doNotifyChanges);
			else
				InnerItem.ClearError("DefaultCurrency");
		}

		/// <summary>
		/// gets all languages for specified store. Function is almost duplicated in CatalogViewModel
		/// </summary>
		/// <param name="catalogId"></param>
		/// <returns></returns>
		private ObservableCollection<StoreLanguageDisplay> GetAvailableLanguages(string storeId, IAppConfigRepository repository)
		{
			var result = new ObservableCollection<StoreLanguageDisplay>();

			var setting = repository.Settings.Where(x => x.Name == SettingName_Languages).ExpandAll().SingleOrDefault();
			if (setting != null)
			{
				foreach (var language in setting.SettingValues)
				{
					var item = EntityFactory.CreateEntity<StoreLanguage>();
					item.LanguageCode = language.ShortTextValue;
					item.StoreId = storeId;

					result.Add(new StoreLanguageDisplay
					{
						Language = item,
						DisplayName = CultureInfo.GetCultureInfo(item.LanguageCode).DisplayName
					});
				}
			}

			return result;
		}

		private ObservableCollection<StoreCurrency> GetAvailableCurrencies(string storeId, IAppConfigRepository repository)
		{
			var result = new ObservableCollection<StoreCurrency>();
			var setting = repository.Settings.Where(x => x.Name == SettingName_Currencies).ExpandAll().SingleOrDefault();
			if (setting != null)
			{
				foreach (var item in setting.SettingValues)
				{
					var storeCurrency = EntityFactory.CreateEntity<StoreCurrency>();
					storeCurrency.CurrencyCode = item.ShortTextValue;
					storeCurrency.StoreId = storeId;
					result.Add(storeCurrency);
				}
			}

			return result;
		}
	}
}
