using System;
using System.Globalization;
using Common.Logging;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Properties;

namespace VirtoCommerce.Framework.Web
{
    /// <summary>
    /// Base class that provides a basic bootstrapping sequence that
    /// registers most of the Prism Library assets
    /// in a <see cref="IUnityContainer"/>.
    /// </summary>
    /// <remarks>
    /// This class must be overridden to provide application specific configuration.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public abstract class UnityBootstrapper : Bootstrapper
    {
        private bool useDefaultConfiguration = true;

        /// <summary>
        /// Gets the default <see cref="IUnityContainer"/> for the application.
        /// </summary>
        /// <value>The default <see cref="IUnityContainer"/> instance.</value>
        [CLSCompliant(false)]
        public IUnityContainer Container { get; protected set; }


        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default Prism Library services in the container. This is the default behavior.</param>
        public override void Run(bool runWithDefaultConfiguration)
        {
            this.useDefaultConfiguration = runWithDefaultConfiguration;

            this.Logger = this.CreateLogger();
            if (this.Logger == null)
            {
                throw new InvalidOperationException(Resources.NullLoggerFacadeException);
            }

            this.Logger.Debug(Resources.LoggerCreatedSuccessfully);

            this.Logger.Debug(Resources.CreatingModuleCatalog);
            this.ModuleCatalog = this.CreateModuleCatalog();
            if (this.ModuleCatalog == null)
            {
                throw new InvalidOperationException(Resources.NullModuleCatalogException);
            }

            this.Logger.Debug(Resources.ConfiguringModuleCatalog);
            this.ConfigureModuleCatalog();

            this.Logger.Debug(Resources.CreatingUnityContainer);
            this.Container = this.CreateContainer();
            if (this.Container == null)
            {
                throw new InvalidOperationException(Resources.NullUnityContainerException);
            }

            this.Logger.Debug(Resources.ConfiguringUnityContainer);
            this.ConfigureContainer();

            this.Logger.Debug(Resources.ConfiguringServiceLocatorSingleton);
            this.ConfigureServiceLocator();

       
            this.Logger.Debug(Resources.RegisteringFrameworkExceptionTypes);
            this.RegisterFrameworkExceptionTypes();

            if (this.Container.IsRegistered<IModuleManager>())
            {
                this.Logger.Debug(Resources.InitializingModules);
                this.InitializeModules();
            }

            this.Logger.Debug(Resources.BootstrapperSequenceCompleted);
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="ServiceLocator" />.
        /// </summary>
        protected override void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => this.Container.Resolve<IServiceLocator>());
        }

        /// <summary>
        /// Registers in the <see cref="IUnityContainer"/> the <see cref="Type"/> of the Exceptions
        /// that are not considered root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();

			//ExceptionExtensions.RegisterFrameworkExceptionType(
			//	typeof(Microsoft.Practices.Unity.ResolutionFailedException));
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            this.Logger.Debug(Resources.AddingUnityBootstrapperExtensionToContainer);
        
            Container.RegisterInstance<ILog>(Logger);

            this.Container.RegisterInstance(this.ModuleCatalog);

            if (useDefaultConfiguration)
            {
                RegisterTypeIfMissing(typeof(IServiceLocator), typeof(UnityServiceLocatorAdapter), true);
                RegisterTypeIfMissing(typeof(IModuleInitializer), typeof(ModuleInitializer), true);
                RegisterTypeIfMissing(typeof(IModuleManager), typeof(ModuleManager), true);
            }
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog
        /// </summary>
        protected override void InitializeModules()
        {
            IModuleManager manager;

            try
            {
                manager = this.Container.Resolve<IModuleManager>();
            }
            catch (ResolutionFailedException ex)
            {
                if (ex.Message.Contains("IModuleCatalog"))
                {
                    throw new InvalidOperationException(Resources.NullModuleCatalogException);
                }

                throw;
            }

            manager.Run();
        }

        /// <summary>
        /// Creates the <see cref="IUnityContainer"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IUnityContainer"/>.</returns>
        [CLSCompliant(false)]
        protected virtual IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        /// <summary>
        /// Registers a type in the container only if that type was not already registered.
        /// </summary>
        /// <param name="fromType">The interface type to register.</param>
        /// <param name="toType">The type implementing the interface.</param>
        /// <param name="registerAsSingleton">Registers the type as a singleton.</param>
        protected void RegisterTypeIfMissing(Type fromType, Type toType, bool registerAsSingleton)
        {
            if (fromType == null)
            {
                throw new ArgumentNullException("fromType");
            }
            if (toType == null)
            {
                throw new ArgumentNullException("toType");
            }
            if (Container.IsTypeRegistered(fromType))
            {
                Logger.Debug(
                    String.Format(CultureInfo.CurrentCulture,
                                  Resources.TypeMappingAlreadyRegistered,
                                  fromType.Name));
            }
            else
            {
                if (registerAsSingleton)
                {
                    Container.RegisterType(fromType, toType, new ContainerControlledLifetimeManager());
                }
                else
                {
                    Container.RegisterType(fromType, toType);
                }
            }
        }
    }
}
