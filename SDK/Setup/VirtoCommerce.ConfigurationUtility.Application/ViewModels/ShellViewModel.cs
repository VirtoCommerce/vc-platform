using System;
using System.Reflection;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ConfigurationUtility.Application.ViewModels
{
    public class ShellViewModel : ViewModelBase, IShellViewModel
    {
        public string AssemblyVersion
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                return String.Format("{0} (Build {1})", assembly.GetInformationalVersion(), assembly.GetFileVersion());
            }
        }
    }
}