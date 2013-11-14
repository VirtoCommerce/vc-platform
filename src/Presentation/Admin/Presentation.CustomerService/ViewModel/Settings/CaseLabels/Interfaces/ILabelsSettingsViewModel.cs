using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseLabels.Interfaces
{
	public interface ILabelsSettingsViewModel : IViewModel
	{

		DelegateCommand AddLabelCommand { get; }
		DelegateCommand<Label> EditLabelCommand { get; }
		DelegateCommand<Label> DeleteLabelCommand { get; }

		InteractionRequest<Confirmation> CommonConfirmRequest { get; }

		ObservableCollection<Label> Items { get; }

		void RaiseCanExecuteChanged();
	}
}
