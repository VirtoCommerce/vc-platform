using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings
{
	public class MultiLineEditViewModel : ViewModelBase, IMultiLineEditViewModel
	{
		public string InnerItem { get; set; }

		#region Constructor

		public MultiLineEditViewModel(string title, string subTitle)
        {
            ViewTitle = new ViewTitleBase() { Title = title, SubTitle = subTitle };
        }


		#endregion
	}
}
