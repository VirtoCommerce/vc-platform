using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class StoreNavigationStepViewModel : StoreViewModel, IStoreNavigationStepViewModel
	{
		public StoreNavigationStepViewModel(IStoreEntityFactory entityFactory, Store item,
			IRepositoryFactory<IStoreRepository> repositoryFactory)
			: base(repositoryFactory, entityFactory, item)
		{
		}

		public StoreSetting SettingFilteredNavigation { get; private set; }

		protected override void InitializePropertiesForViewing()
		{
			// setting "FilteredBrowsing"
			var textSettingNameFilteredNavigation = "FilteredBrowsing";
			var listSettings =
				InnerItem.Settings.Where(
					x => x.ValueType == StoreSettingViewModel.textXML && x.Name == textSettingNameFilteredNavigation).ToList();
			if (listSettings.Count == 0)
			{
				SettingFilteredNavigation = EntityFactory.CreateEntity<StoreSetting>();
				SettingFilteredNavigation.Name = textSettingNameFilteredNavigation;
				SettingFilteredNavigation.ValueType = StoreSettingViewModel.textXML;
				//SettingEnableCVV.LongTextValue = "";
				InnerItem.Settings.Add(SettingFilteredNavigation);
			}
			else
				SettingFilteredNavigation = listSettings[0];
			OnPropertyChanged("SettingFilteredNavigation");
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var retval = true;
				return retval;
			}
		}

		public override bool IsLast
		{
			get
			{
				return true;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter navigation information.".Localize();
			}
		}
		#endregion

	}
}
