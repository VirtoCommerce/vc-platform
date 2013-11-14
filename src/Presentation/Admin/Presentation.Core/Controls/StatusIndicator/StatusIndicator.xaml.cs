using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.ViewModel;

namespace VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator
{
    public partial class StatusIndicator
    {
        public StatusIndicator()
        {
	        DataContext = StatusIndicatorViewModel.GetInstance();

            InitializeComponent();
        }
    }
}
