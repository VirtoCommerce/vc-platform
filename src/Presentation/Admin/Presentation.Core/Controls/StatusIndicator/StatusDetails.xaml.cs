using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.ViewModel;

namespace VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator
{
	public partial class StatusDetails
	{
		public StatusDetails()
		{
			DataContext = StatusIndicatorViewModel.GetInstance();

			InitializeComponent();
		}
	}
}
