using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Stores.Factories;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Interfaces;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.FulfillmentCenters.Implementations
{
	public class FulfillmentCentersSettingsViewModel : HomeSettingsEditableViewModel<FulfillmentCenter>, IFulfillmentCentersSettingsViewModel
	{

		#region Dependencies

		private readonly IRepositoryFactory<IFulfillmentCenterRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public FulfillmentCentersSettingsViewModel(IRepositoryFactory<IFulfillmentCenterRepository> repositoryFactory, IStoreEntityFactory entityFactory, IViewModelsFactory<ICreateFulfillmentCenterViewModel> wizardVmFactory, IViewModelsFactory<IFulfillmentCenterViewModel> editVmFactory)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region HomeSettingsViewModel override members


		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.FulfillmentCenters.OrderBy(fc => fc.Name).ToList();
					return items;
				}
			}

			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as FulfillmentCenter;
			if (itemToUpdate != null)
			{
				FulfillmentCenter itemFromInnerItem =
					Items.SingleOrDefault(fc => fc.FulfillmentCenterId == itemToUpdate.FulfillmentCenterId);

				if (itemFromInnerItem != null)
				{
					OnUIThread(() =>
					{
						itemFromInnerItem.InjectFrom<CloneInjection>(itemToUpdate);
						OnPropertyChanged("Items");
					});
				}
			}
		}

		#endregion

		#region HomeSettingsEditableViewModel override Members

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<FulfillmentCenter>();
			item.MaxReleasesPerPickBatch = 20;
			item.PickDelay = 30;

			var vm = WizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item)
				);
			var confirmation = new ConditionalConfirmation()
				{
					Title = "Create fulfillment center".Localize(),
					Content = vm
				};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(FulfillmentCenter item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this)
				);
			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(FulfillmentCenter item)
		{
			var confirmation = new ConditionalConfirmation()
				{
					Content = string.Format("Are you sure you want to delete Fulfillment center '{0}'?".Localize(), item.Name),
					Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
				};
			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion
	}
}
