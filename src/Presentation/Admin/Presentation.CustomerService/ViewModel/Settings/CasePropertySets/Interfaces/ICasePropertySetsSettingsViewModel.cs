using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces
{
    public interface ICasePropertySetsSettingsViewModel : IViewModel
    {
        ICollectionView ItemsView { get; }

        DelegateCommand<CasePropertySet> ItemMoveUpCommand { get; }
        DelegateCommand<CasePropertySet> ItemMoveDownCommand { get; }
    }
}
