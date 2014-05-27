using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Implementations
{
	public class CasePropertyViewModel : ViewModelBase, ICasePropertyViewModel
	{
		#region Constructor

		public CasePropertyViewModel(CaseProperty item)
		{
            ViewTitle = new ViewTitleBase() { Title = "Edit Info Value", SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory) };
			InnerItem = item;
		}



		#endregion

		public CaseProperty InnerItem { get; private set; }
	}
}
