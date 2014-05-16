using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class StoreSettingsStepViewModel : StoreViewModel, IStoreSettingStepViewModel
	{

		#region Dependencies

		private readonly IViewModelsFactory<IStoreSettingViewModel> _settingVmFactory;

		#endregion

		#region Constructor

		public StoreSettingsStepViewModel(IStoreEntityFactory entityFactory, Store item,
			IRepositoryFactory<IStoreRepository> repositoryFactory, IViewModelsFactory<IStoreSettingViewModel> vmFactory)
			: base(repositoryFactory, entityFactory, item)
		{
			_settingVmFactory = vmFactory;
		}

		#endregion

		#region Commands

		//		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		public DelegateCommand StoreSettingAddCommand { get; private set; }
		public DelegateCommand<StoreSetting> StoreSettingEditCommand { get; private set; }
		public DelegateCommand<StoreSetting> StoreSettingDeleteCommand { get; private set; }

		#endregion

		protected override void InitializePropertiesForViewing()
		{
			if (StoreSettingAddCommand == null)
			{
				StoreSettingAddCommand = new DelegateCommand(RaiseStoreSettingAddInteractionRequest);
				StoreSettingEditCommand = new DelegateCommand<StoreSetting>(RaiseStoreSettingEditInteractionRequest, x => x != null);
				StoreSettingDeleteCommand = new DelegateCommand<StoreSetting>(RaiseStoreSettingDeleteInteractionRequest, x => x != null);
				//CommonConfirmRequest = new InteractionRequest<Confirmation>();
			}
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
				return string.Empty;
			}
		}
		#endregion

		#region STORE SETTING tab

		public void StoreSettingRaiseCanExecuteChanged()
		{
			StoreSettingEditCommand.RaiseCanExecuteChanged();
			StoreSettingDeleteCommand.RaiseCanExecuteChanged();
		}

		private void RaiseStoreSettingAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<StoreSetting>();
			item.StoreId = InnerItem.StoreId;
			if (RaiseStoreSettingEditInteractionRequest(item, "Add Store Setting".Localize()))
			{
				InnerItem.Settings.Add(item);
			}
		}

		private void RaiseStoreSettingEditInteractionRequest(StoreSetting originalItem)
		{
			var item = originalItem.DeepClone(EntityFactory as IKnownSerializationTypes);
			if (RaiseStoreSettingEditInteractionRequest(item, "Edit Store Setting".Localize()))
			{

				// copy all values to original:
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
				originalItem.ValueType = originalItem.ValueType;

				IsModified = true;
			}
		}

		private bool RaiseStoreSettingEditInteractionRequest(StoreSetting item, string title)
		{
			bool result = false;
			var itemVM = _settingVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item)
				);
			var confirmation = new ConditionalConfirmation(itemVM.InnerItem.Validate);
			confirmation.Title = title;
			confirmation.Content = itemVM;

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});

			return result;
		}

		private void RaiseStoreSettingDeleteInteractionRequest(StoreSetting item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Store Setting '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					InnerItem.Settings.Remove(item);
				}
			});
		}
		#endregion

	}
}
