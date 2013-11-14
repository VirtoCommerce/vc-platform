using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Interfaces
{
    public interface ICreateCaseRuleViewModel : IViewModel
    {
		TypedExpressionElementBase ExpressionElementBlock { get; set; }
    }

    public interface ICaseRuleOverviewStepViewModel : IWizardStep
    {
        void PrepareOriginalItemForSave();
    }
}
