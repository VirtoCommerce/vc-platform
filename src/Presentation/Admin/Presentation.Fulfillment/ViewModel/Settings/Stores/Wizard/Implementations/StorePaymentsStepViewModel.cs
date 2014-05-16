using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class StorePaymentsStepViewModel : StoreViewModel, IStorePaymentsStepViewModel, ISupportWizardPrepare
	{
		#region Dependencies

		private readonly IRepositoryFactory<IPaymentMethodRepository> _paymentRepositoryFactory;
		private readonly IRepositoryFactory<IStoreRepository> _repositoryFactory;

		#endregion

		public StorePaymentsStepViewModel(IStoreEntityFactory entityFactory, Store item,
			 IRepositoryFactory<IStoreRepository> repositoryFactory,
			IRepositoryFactory<IPaymentMethodRepository> paymentRepositoryFactory)
			: base(repositoryFactory, entityFactory, item)
		{
			_paymentRepositoryFactory = paymentRepositoryFactory;
			_repositoryFactory = repositoryFactory;
		}


		#region Properties

		public List<StorePaymentGatewayViewModel> AvailableStorePaymentGateways { get; private set; }
		public List<StoreCardTypeViewModel> AvailableStoreCardTypes { get; private set; }
		public StoreSetting SettingEnableCVV { get; private set; }

		#endregion

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
				return "Enter payments information.".Localize();
			}
		}
		#endregion


		protected override void InitializePropertiesForViewing()
		{
			using (var repository = _paymentRepositoryFactory.GetRepositoryInstance())
			{
				AvailableStorePaymentGateways = repository.PaymentMethods.OrderBy(x => x.Name).ToArray().Select(x =>
				{
					var gateway = EntityFactory.CreateEntity<StorePaymentGateway>();
					gateway.PaymentGateway = x.Name;
					gateway.StoreId = InnerItem.StoreId;
					return new StorePaymentGatewayViewModel(gateway, x.Name, _repositoryFactory.GetRepositoryInstance());
				}).ToList();
				OnPropertyChanged("AvailableStorePaymentGateways");
			}

			AvailableStoreCardTypes = GetAvailableStoreCardTypes(this, _repositoryFactory.GetRepositoryInstance());
			OnPropertyChanged("AvailableStoreCardTypes");

			// setting "CVV Code Validation"
			string textSettingNameEnableCvvCodeValidation = "CVV";
			var listSettingsCVV = InnerItem.Settings.Where(x => x.ValueType == StoreSettingViewModel.textBoolean && x.Name == textSettingNameEnableCvvCodeValidation).ToList();
			if (listSettingsCVV.Count == 0)
			{
				SettingEnableCVV = EntityFactory.CreateEntity<StoreSetting>();
				SettingEnableCVV.Name = textSettingNameEnableCvvCodeValidation;
				SettingEnableCVV.ValueType = StoreSettingViewModel.textBoolean;
				SettingEnableCVV.BooleanValue = false;
				InnerItem.Settings.Add(SettingEnableCVV);
			}
			else
				SettingEnableCVV = listSettingsCVV[0];
			OnPropertyChanged("SettingEnableCVV");
		}

		private static List<StoreCardTypeViewModel> GetAvailableStoreCardTypes(StoreViewModel parentVM,
			IStoreRepository repository)
		{
			var result = new List<StoreCardTypeViewModel>();
			result.Add(
				new StoreCardTypeViewModel(new StoreCardType() { StoreId = parentVM.InnerItem.StoreId, CardType = "Visa" },
					repository));
			result.Add(
				new StoreCardTypeViewModel(
					new StoreCardType() { StoreId = parentVM.InnerItem.StoreId, CardType = "MasterCard" }, repository));
			result.Add(
				new StoreCardTypeViewModel(new StoreCardType() { StoreId = parentVM.InnerItem.StoreId, CardType = "Amex" },
					repository));
			return result;
		}

		#region PAYMENTS tab
		public class StorePaymentGatewayViewModel : ViewModelBase
		{
			public StorePaymentGatewayViewModel(StorePaymentGateway item, string displayName, IStoreRepository repository)
			{
				InnerItem = item;
				_DisplayName = displayName;

				var storeGateways = repository.StorePaymentGateways.Where(
					stc => stc.PaymentGateway == InnerItem.PaymentGateway && stc.StoreId == InnerItem.StoreId)
					.SingleOrDefault();
				if (storeGateways != null)
				{
					IsChecked = true;
				}
				else
				{
					IsChecked = false;
				}
			}

			public StorePaymentGateway InnerItem { get; private set; }

			private string _DisplayName;
			public override string DisplayName
			{
				get
				{
					return _DisplayName;
				}
			}

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

		public class StoreCardTypeViewModel : ViewModelBase
		{

			public StoreCardTypeViewModel(StoreCardType item, IStoreRepository repository)
			{
				InnerItem = item;

				var cardTypeFromDb = repository.StoreCardTypes.Where(
					stc => stc.CardType == InnerItem.CardType && stc.StoreId == InnerItem.StoreId).SingleOrDefault();
				if (cardTypeFromDb != null)
				{
					IsChecked = true;
				}
				else
				{
					IsChecked = false;
				}
			}

			public StoreCardType InnerItem { get; private set; }

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
		#endregion

		#region ISupportWizardPrepare members

		public void Prepare()
		{
			UpdateCardTypesList();
			UpdateStoreGatewaysList();
		}

		#endregion

		#region Private methods

		private void UpdateCardTypesList()
		{
			if (AvailableStoreCardTypes == null)
				return;

			var itemsToAdd = AvailableStoreCardTypes.Where(x => x.IsChecked == true).ToList();
			foreach (var itemToAdd in itemsToAdd)
			{

				InnerItem.CardTypes.Add(itemToAdd.InnerItem);
			}
		}

		private void UpdateStoreGatewaysList()
		{
			if (AvailableStorePaymentGateways == null)
				return;

			var itemsToAdd = AvailableStorePaymentGateways.Where(x => x.IsChecked == true).ToList();
			foreach (var itemToAdd in itemsToAdd)
			{

				InnerItem.PaymentGateways.Add(itemToAdd.InnerItem);

			}
		}

		#endregion

	}
}
