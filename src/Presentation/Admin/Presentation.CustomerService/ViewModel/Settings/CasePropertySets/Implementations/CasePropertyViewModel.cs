using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Implementations
{
    public class CasePropertyViewModel : ViewModelBase, ICasePropertyViewModel
	{
		#region Constructor

        public CasePropertyViewModel(CaseProperty item)
        {
            ViewTitle = new ViewTitleBase() { Title = "Edit Info Value", SubTitle = "SETTINGS" };
			InnerItem = item;
        }

        

		#endregion

        public CaseProperty InnerItem { get; private set; }
	}
}
