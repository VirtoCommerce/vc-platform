using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Implementations
{
	public class TaxSettingsViewModel : HomeSettingsEditableViewModel<Tax>, ITaxSettingsViewModel
	{

		#region Dependencies

		private readonly IRepositoryFactory<ITaxRepository> _repositoryFactory;

		#endregion

		public TaxSettingsViewModel(IRepositoryFactory<ITaxRepository> repositoryFactory, IOrderEntityFactory entityFactory, IViewModelsFactory<ICreateTaxViewModel> wizardVmFactory, IViewModelsFactory<ITaxViewModel> editVmFactory)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_repositoryFactory = repositoryFactory;
		}



		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.Taxes.OrderBy(t => t.Name).ToList();
					return items;
				}
			}
			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as Tax;
			if (itemToUpdate != null)
			{
				Tax itemFromInnerItem =
					Items.SingleOrDefault(t => t.TaxId == itemToUpdate.TaxId);

				if (itemFromInnerItem != null)
				{
					OnUIThread(() => itemFromInnerItem.InjectFrom<CloneInjection>(itemToUpdate));
					OnUIThread(() => OnPropertyChanged("Items"));
				}
			}
		}

		#endregion


		#region HomeSettingsEditableViewModel members

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<Tax>();

			var vm = WizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item));

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create tax".Localize(),
				Content = vm
			};
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				ItemAdd(item, confirmation, repository);
			}
		}

		protected override void RaiseItemEditInteractionRequest(Tax item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(Tax item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Tax '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				ItemDelete(item, confirmation, repository);
			}
		}

		#endregion
	}
}
