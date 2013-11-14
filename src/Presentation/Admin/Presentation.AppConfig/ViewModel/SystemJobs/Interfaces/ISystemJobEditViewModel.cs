using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Interfaces
{
	public interface ISystemJobEditViewModel: IViewModel
    {
		SystemJob InnerItem { get; set; }

		DelegateCommand ItemAddCommand { get; }
		DelegateCommand<JobParameter> ItemEditCommand { get; }
		DelegateCommand<JobParameter> ItemDeleteCommand { get; }
		InteractionRequest<ConditionalConfirmation> RemoveConfirmRequest { get; }
    }
}
