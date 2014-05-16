using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Implementations;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Taxes.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Implementations
{
	public class CreateTaxViewModel : WizardContainerStepsViewModel, ICreateTaxViewModel
	{
		public CreateTaxViewModel(IViewModelsFactory<ITaxOverviewStepViewModel> overviewVmFactory, IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory, Tax item)
		{
			RegisterStep(overviewVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item), new KeyValuePair<string, object>("isWizardMode", true)));
			RegisterStep(langVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("selectedLanguages", item.TaxLanguages)));
		}

		#region WizardContainerStepsViewModel

		protected override void BeforePrepareSteps()
		{
			var languagesVM = AllRegisteredSteps.First(x => x is IGeneralLanguagesStepViewModel) as IGeneralLanguagesStepViewModel;
			var taxVM = AllRegisteredSteps.First(x => x is ITaxViewModel) as ITaxViewModel;
			taxVM.UpdateOfLanguages(languagesVM);
		}

		#endregion
	}

	public class TaxOverviewStepViewModel : TaxViewModel, ITaxOverviewStepViewModel
	{
		public TaxOverviewStepViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IOrderEntityFactory entityFactory, IViewModelsFactory<IGeneralLanguagesStepViewModel> langVmFactory,
			IViewModelsFactory<ITaxValueViewModel> valueVmFactory, Tax item)
			: base(repositoryFactory, entityFactory, langVmFactory, valueVmFactory, item)
		{
		}

		#region IWizardStep Members

		public override string Description
		{
			get { return "Enter Tax general information.".Localize(); }
		}

		#endregion
	}
}
