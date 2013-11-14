using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Order.Model;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces
{
	public interface ICreateRefundViewModel : IViewModel
	{
		CreateRefundModel InnerModel { get; }

		Action OnAfterSuccessfulSubmit { set; }
	}

	public interface IRefundDetailsStepViewModel : IWizardStep
	{
	}

	public interface IRefundSummaryStepViewModel : IWizardStep
	{
	}
}
