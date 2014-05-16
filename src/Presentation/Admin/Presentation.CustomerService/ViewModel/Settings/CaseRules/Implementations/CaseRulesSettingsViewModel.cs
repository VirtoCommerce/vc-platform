using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Implementations
{
	public class CaseRulesSettingsViewModel : HomeSettingsEditableViewModel<CaseRule>, ICaseRulesSettingsViewModel
	{

		#region Dependencies

		private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public CaseRulesSettingsViewModel(IRepositoryFactory<ICustomerRepository> repositoryFactory, ICustomerEntityFactory entityFactory, IViewModelsFactory<ICreateCaseRuleViewModel> wizardVMFactory, IViewModelsFactory<ICaseRuleViewModel> editVMFactory)
			: base(entityFactory, wizardVMFactory, editVMFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.CaseRules.OrderBy(cr => cr.Name).ToList();
					return items;
				}
			}

			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as CaseRule;
			if (itemToUpdate != null)
			{
				CaseRule itemFromInnerItem =
					Items.SingleOrDefault(cr => cr.CaseRuleId == itemToUpdate.CaseRuleId);

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

		#region HomeSettingsEditableViewModel members

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<CaseRule>();

			var vm = WizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create case rule".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(CaseRule item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(CaseRule item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Case rule '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion







	}
}
