using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public interface IHomeSettingsViewModel
	{
		DelegateCommand RefreshItemListCommand { get; }

		void RaiseCanExecuteChanged();
		void RefreshItem(object item);
	}
}
