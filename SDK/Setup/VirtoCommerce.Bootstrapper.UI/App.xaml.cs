using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System.Windows;
using StartupEventArgs = System.Windows.StartupEventArgs;

namespace VirtoCommerce.Bootstrapper.UI
{
    public partial class App
    {
        private readonly BootstrapperApplication _installer;

        public App(BootstrapperApplication installer)
        {
            _installer = installer;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new Bootstrapper(_installer);
            bootstrapper.Run();
        }
    }
}
