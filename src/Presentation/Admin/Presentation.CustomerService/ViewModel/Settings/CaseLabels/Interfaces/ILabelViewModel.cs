using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseLabels.Interfaces
{
	public interface ILabelViewModel : IViewModel
	{
        Label InnerItem { get; }
	}
}
