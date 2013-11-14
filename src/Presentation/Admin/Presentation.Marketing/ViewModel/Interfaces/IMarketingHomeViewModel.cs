using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.ComponentModel;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces
{
    public interface IMarketingHomeViewModel : IViewModel
    {
        DelegateCommand PromotionItemCreateCommand { get; }
        DelegateCommand PromotionCartCreateCommand { get; }
        DelegateCommand SearchItemsCommand { get; }
        ICollectionView ListItemsSource { get; }

        InteractionRequest<Confirmation> CommonWizardDialogRequest { get; }
    }
}
