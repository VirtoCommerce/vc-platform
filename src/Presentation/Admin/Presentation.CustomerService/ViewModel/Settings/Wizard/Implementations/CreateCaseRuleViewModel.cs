using System.Collections.Generic;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Implementations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Implementations
{
	public class CreateCaseRuleViewModel : WizardContainerStepsViewModel, ICreateCaseRuleViewModel
	{
		public TypedExpressionElementBase ExpressionElementBlock { get; set; }

		public CreateCaseRuleViewModel(IViewModelsFactory<ICaseRuleOverviewStepViewModel> overviewVmFactory, CaseRule item)
		{
			var parameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(overviewVmFactory.GetViewModelInstance(parameter));
		}
	}

	public class CaseRuleOverviewStepViewModel : CaseRuleViewModel, ICaseRuleOverviewStepViewModel, ISupportWizardPrepare
	{
		public CaseRuleOverviewStepViewModel(IViewModelsFactory<IMultiLineEditViewModel> multiLineEditVmFactory, IRepositoryFactory<ICustomerRepository> repositoryFactory, ICustomerEntityFactory entityFactory, CaseRule item)
			: base(multiLineEditVmFactory, repositoryFactory, entityFactory, item)
		{
		}

		public override string Description
		{
			get
			{
				var result = "Enter rule details".Localize();
				return result;
			}
		}

		#region ISupportWizardPrepare

		//Update InnerItem from the step
		public void Prepare()
		{
			PrepareOriginalItemForSave();
		}

		#endregion
	}
}
