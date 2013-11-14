using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Interfaces
{
    public interface ICaseRuleViewModel : IViewModel, IExpressionViewModel
    {
        CaseRule InnerItem { get; }
		IViewModelsFactory<IMultiLineEditViewModel> MultiLineEditVmFactory { get; }
        void PrepareOriginalItemForSave();
    }
}
