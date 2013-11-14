using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.Bootstrapper.Main;
using VirtoCommerce.Bootstrapper.UI.Properties;
using VirtoCommerce.Bootstrapper.UI.ViewModels;
using VirtoCommerce.Bootstrapper.UI.Views;

namespace VirtoCommerce.Bootstrapper.UI
{
    public sealed class Bootstrapper : UnityBootstrapper
    {
        private readonly BootstrapperApplication _installer;
        private readonly bool _isInTestMode;

        public Bootstrapper(BootstrapperApplication installer)
        {
            _installer = installer;
        }

        public Bootstrapper()
        {
            _isInTestMode = true;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var mainModule = typeof(MainModule);
            ModuleCatalog.AddModule(new ModuleInfo
                                        {
                                            ModuleName = mainModule.Name,
                                            ModuleType = mainModule.AssemblyQualifiedName,
                                            Ref = new Uri(mainModule.Assembly.Location).AbsoluteUri,
                                            InitializationMode = InitializationMode.WhenAvailable,
                                        });
        }

        protected override void ConfigureContainer()
        {
            if (IsUILoaded)
            {
                Container.RegisterType<NavigationManager, NavigationManager>(new ContainerControlledLifetimeManager());
            }

            base.ConfigureContainer();

            if (_installer != null)
            {
                Container.RegisterInstance(_installer, new ContainerControlledLifetimeManager());
                Container.RegisterInstance(_installer.Engine, new ContainerControlledLifetimeManager());
            }
        }

        //// Calling ConfigureServiceLocator

        //// Calling ConfigureRegionAdapterMappings

        //// Calling ConfigureDefaultRegionBehaviors

        //// Calling RegisterFrameworkExceptionTypes

        protected override DependencyObject CreateShell()
        {
            DependencyObject ret = null;
            
            if (_installer != null)
            {
                ret = _installer.Command.Display == Display.Passive || _installer.Command.Display == Display.Full ? new Shell() : null;
            }
            else
            {
                ret = new Shell();
            }

            return ret;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            if (IsUILoaded)
            {
                if (!_isInTestMode)
                {
                    var engine = Container.Resolve<Engine>();
                    engine.Log(LogLevel.Verbose, Resources.CreatingUI);
                }

                var shell = (Window)Shell;
                shell.DataContext = new ShellViewModel();
                shell.Show();
            }
        }

        private bool IsUILoaded
        {
            get
            {
                var loadUI = true;

                if (_installer != null && (_installer.Command.Display == Display.Passive || _installer.Command.Display == Display.Full))
                {
                    loadUI = true;
                }
                else
                {
                    loadUI = false;
                }

                if (_installer == null && _isInTestMode)
                {
                    loadUI = true;
                }

                return loadUI;
            }
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            if (!_isInTestMode)
            {
                _installer.Engine.Detect();
            }
        }

        //// Calling Run
    }
}