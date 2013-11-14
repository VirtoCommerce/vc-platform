using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VirtoCommerce.Bootstrapper.UI;

namespace VirtoCommerce.InstallTester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var installer = new TestBootStrapperApp();
            var a = new VirtoCommerce.Bootstrapper.UI.Bootstrapper();
            a.Run();

            //var exitCode = new VirtoCommerce.Bootstrapper.UI.App(new Installer()).Run();
        }
    }

    public class TestBootStrapperApp : Microsoft.Tools.WindowsInstallerXml.Bootstrapper.BootstrapperApplication
    {
        protected override void Run()
        {
            //this.Engine = new Microsoft.Tools.WindowsInstallerXml.Bootstrapper.Engine();
        }
    }
}
