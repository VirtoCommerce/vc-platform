using VirtoCommerce.ManagementClient.Core.Infrastructure;

using VirtoCommerce.Bootstrapper.Main.Properties;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
    public class HelpViewModel : ViewModelBase, IHelpViewModel
    {
        public HelpViewModel()
        {
            ViewTitle = new ViewTitleBase { Title = Resources.SDKTitle, SubTitle = Resources.HelpTitle };
        }
    }
}