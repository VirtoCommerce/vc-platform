using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class StoreTaxesStepViewModel : StoreViewModel, IStoreTaxesStepViewModel, ISupportWizardPrepare
	{

		#region Dependencies

		private readonly IRepositoryFactory<IStoreRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public StoreTaxesStepViewModel(IStoreEntityFactory entityFactory, Store item,
			 IRepositoryFactory<IStoreRepository> repositoryFactory)
			: base(repositoryFactory, entityFactory, item)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region Properties

		public StoreTaxJurisdictionViewModel[] AvailableTaxJurisdictions { get; private set; }
		public StoreTaxCodeViewModel[] AvailableTaxCodes { get; private set; }

		#endregion

		protected override void InitializePropertiesForViewing()
		{
			AvailableTaxJurisdictions = GetAvailableTaxJurisdictions(this, _repositoryFactory.GetRepositoryInstance());
			OnPropertyChanged("AvailableTaxJurisdictions");
			AvailableTaxCodes = GetAvailableTaxCodes(this, _repositoryFactory.GetRepositoryInstance());
			OnPropertyChanged("AvailableTaxCodes");
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
				return false;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter taxes information.".Localize();
			}
		}
		#endregion

		private static StoreTaxJurisdictionViewModel[] GetAvailableTaxJurisdictions(StoreViewModel parentVM,
			IStoreRepository repository)
		{
			var result = new StoreTaxJurisdictionViewModel[]
	        {
	            new StoreTaxJurisdictionViewModel(
	                new StoreTaxJurisdiction() {StoreId = parentVM.InnerItem.StoreId, TaxJurisdiction = "China"},
	                repository),
	            new StoreTaxJurisdictionViewModel(
	                new StoreTaxJurisdiction() {StoreId = parentVM.InnerItem.StoreId, TaxJurisdiction = "Canada"},
	                repository),
	            new StoreTaxJurisdictionViewModel(
	                new StoreTaxJurisdiction() {StoreId = parentVM.InnerItem.StoreId, TaxJurisdiction = "USA"}, repository)
	        };

			return result;
		}

		private static StoreTaxCodeViewModel[] GetAvailableTaxCodes(StoreViewModel parentVM, IStoreRepository repository)
		{
			var result = new[]
	        {
	            new StoreTaxCodeViewModel(new StoreTaxCode() {StoreId = parentVM.InnerItem.StoreId, TaxCode = "Shipping"},
	                repository),
	            new StoreTaxCodeViewModel(new StoreTaxCode() {StoreId = parentVM.InnerItem.StoreId, TaxCode = "Goods"},
	                repository),
	            new StoreTaxCodeViewModel(new StoreTaxCode() {StoreId = parentVM.InnerItem.StoreId, TaxCode = "none"},
	                repository)
	        };
			return result;
		}

		public class StoreTaxJurisdictionViewModel : ViewModelBase
		{

			public StoreTaxJurisdictionViewModel(StoreTaxJurisdiction item, IStoreRepository repository)
			{
				InnerItem = item;

				var jurisFromDb = repository.StoreTaxJurisdictions.Where(
					stc => stc.TaxJurisdiction == InnerItem.TaxJurisdiction && stc.StoreId == InnerItem.StoreId)
					.SingleOrDefault();
				if (jurisFromDb != null)
				{
					IsChecked = true;
				}
				else
				{
					IsChecked = false;
				}
			}

			public StoreTaxJurisdiction InnerItem { get; private set; }


			private bool _isChecked;
			public bool IsChecked
			{
				get { return _isChecked; }
				set
				{
					_isChecked = value;
					OnPropertyChanged();
				}
			}
		}

		public class StoreTaxCodeViewModel : ViewModelBase
		{
			public StoreTaxCodeViewModel(StoreTaxCode item, IStoreRepository repository)
			{
				InnerItem = item;

				var codeFromDb = repository.StoreTaxCodes.Where(
					stc => stc.TaxCode == InnerItem.TaxCode && stc.StoreId == InnerItem.StoreId).SingleOrDefault();
				if (codeFromDb != null)
				{
					IsChecked = true;
				}
				else
				{
					IsChecked = false;
				}
			}

			public StoreTaxCode InnerItem { get; private set; }

			private bool _isChecked;
			public bool IsChecked
			{
				get { return _isChecked; }
				set
				{
					_isChecked = value;
					OnPropertyChanged();
				}
			}


		}

		#region ISupportWizardStep members

		public void Prepare()
		{
			UpdateTaxJurisdictionsList();
			UpdateTaxCodeList();
		}

		#endregion

		#region PrivateMethods

		private void UpdateTaxJurisdictionsList()
		{
			if (AvailableTaxJurisdictions == null)
				return;

			var itemsToAdd = AvailableTaxJurisdictions.Where(x => x.IsChecked == true).ToList();
			foreach (var itemToAdd in itemsToAdd)
			{

				InnerItem.TaxJurisdictions.Add(itemToAdd.InnerItem);
			}
		}

		private void UpdateTaxCodeList()
		{
			if (AvailableTaxCodes == null)
				return;

			var itemsToAdd = AvailableTaxCodes.Where(x => x.IsChecked == true).ToList();
			foreach (var itemToAdd in itemsToAdd)
			{

				InnerItem.TaxCodes.Add(itemToAdd.InnerItem);

			}
		}

		#endregion
	}
}
