using System.Collections;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Importing.Model;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces
{
    public interface IImportJobHomeViewModel : IViewModel
    {
		DelegateCommand ClearFiltersCommand { get; }
		DelegateCommand<object> ImportJobRunCommand { get; }
		string DefaultImporter { get; }
		ImportEntityType[] AvailableImporters { get; set; }
    }
}
