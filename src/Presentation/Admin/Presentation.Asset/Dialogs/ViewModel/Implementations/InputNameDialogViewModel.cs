using VirtoCommerce.ManagementClient.Asset.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Asset.Dialogs.ViewModel.Implementations
{
	public class InputNameDialogViewModel : ViewModelBase, IInputNameDialogViewModel
	{
		public string InputText { get; set; }
		public string InputLabel { get; set; }

		public InputNameDialogViewModel()
		{
			InputText = string.Empty;
			InputLabel = "Name".Localize();
		}
	}
}
