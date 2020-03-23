using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Modules
{
    /// <summary>
    /// Component responsible for coordinating the modules' type loading and module initialization process. 
    /// </summary>
    public class ModuleManager : IModuleManager, IDisposable
    {
        private readonly IModuleInitializer _moduleInitializer;
        private readonly ILogger<ModuleManager> _loggerFacade;
        private IEnumerable<IModuleTypeLoader> _typeLoaders;
        private readonly IAssemblyResolver _assemblyResolver;
        private readonly HashSet<IModuleTypeLoader> subscribedToModuleTypeLoaders = new HashSet<IModuleTypeLoader>();

        /// <summary>
        /// Initializes an instance of the <see cref="ModuleManager"/> class.
        /// </summary>
        /// <param name="moduleInitializer">Service used for initialization of modules.</param>
        /// <param name="moduleCatalog">Catalog that enumerates the modules to be loaded and initialized.</param>
        /// <param name="loggerFacade">Logger used during the load and initialization of modules.</param>
        public ModuleManager(IModuleInitializer moduleInitializer, IModuleCatalog moduleCatalog, IAssemblyResolver assemblyResolver, ILogger<ModuleManager> loggerFacade)
        {
            _moduleInitializer = moduleInitializer ?? throw new ArgumentNullException("moduleInitializer");
            ModuleCatalog = moduleCatalog ?? throw new ArgumentNullException("moduleCatalog");
            _loggerFacade = loggerFacade ?? throw new ArgumentNullException("loggerFacade");
            _assemblyResolver = assemblyResolver;
        }

        /// <summary>
        /// The module catalog specified in the constructor.
        /// </summary>
        protected IModuleCatalog ModuleCatalog { get; }


        /// <summary>
        /// Raised repeatedly to provide progress as modules are loaded in the background.
        /// </summary>
        public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        private void RaiseModuleDownloadProgressChanged(ModuleDownloadProgressChangedEventArgs e)
        {
            if (this.ModuleDownloadProgressChanged != null)
            {
                this.ModuleDownloadProgressChanged(this, e);
            }
        }

        /// <summary>
        /// Raised when a module is loaded or fails to load.
        /// </summary>
        public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

        private void RaiseLoadModuleCompleted(ModuleInfo moduleInfo, Exception error)
        {
            this.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, error));
        }

        private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
        {
            if (this.LoadModuleCompleted != null)
            {
                this.LoadModuleCompleted(this, e);
            }
        }

        /// <summary>
        /// Initializes the modules marked as <see cref="InitializationMode.WhenAvailable"/> on the <see cref="ModuleCatalog"/>.
        /// </summary>
        public void Run()
        {
            this.ModuleCatalog.Initialize();

            this.LoadModulesWhenAvailable();
        }


        /// <summary>
        /// Loads and initializes the module on the <see cref="ModuleCatalog"/> with the name <paramref name="moduleName"/>.
        /// </summary>
        /// <param name="moduleName">Name of the module requested for initialization.</param>
        public void LoadModule(string moduleName)
        {
            IEnumerable<ModuleInfo> module = this.ModuleCatalog.Modules.Where(m => m.ModuleName == moduleName);
            if (module == null || module.Count() != 1)
            {
                throw new ModuleNotFoundException(moduleName, $"Module { moduleName } was not found in the catalog.");
            }

            IEnumerable<ModuleInfo> modulesToLoad = this.ModuleCatalog.CompleteListWithDependencies(module);

            this.LoadModuleTypes(modulesToLoad);
        }

        public void PostInitializeModule(ModuleInfo moduleInfo, IApplicationBuilder applicationBuilder)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException("moduleInfo");

            _moduleInitializer.PostInitialize(moduleInfo, applicationBuilder);
        }

        /// <summary>
        /// Returns the list of registered <see cref="IModuleTypeLoader"/> instances that will be 
        /// used to load the types of modules. 
        /// </summary>
        /// <value>The module type loaders.</value>
        public virtual IEnumerable<IModuleTypeLoader> ModuleTypeLoaders
        {
            get
            {
                if (this._typeLoaders == null)
                {
                    this._typeLoaders = new List<IModuleTypeLoader>
                                          {
                                              new FileModuleTypeLoader(_assemblyResolver)
                                          };
                }

                return this._typeLoaders;
            }

            set
            {
                this._typeLoaders = value;
            }
        }

        /// <summary>
        /// Checks if the module needs to be retrieved before it's initialized.
        /// </summary>
        /// <param name="moduleInfo">Module that is being checked if needs retrieval.</param>
        /// <returns></returns>
        protected virtual bool ModuleNeedsRetrieval(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException("moduleInfo");
            }

            if (moduleInfo.Assembly == null)
            {
                return true;
            }

            if (moduleInfo.State == ModuleState.NotStarted)
            {
                // If we can instantiate the type, that means the module's assembly is already loaded into 
                // the AppDomain and we don't need to retrieve it. 
                bool isAvailable = Type.GetType(moduleInfo.ModuleType) != null;
                if (isAvailable)
                {
                    moduleInfo.State = ModuleState.ReadyForInitialization;
                }

                return !isAvailable;
            }

            return false;
        }

        private void LoadModulesWhenAvailable()
        {
            IEnumerable<ModuleInfo> whenAvailableModules = this.ModuleCatalog.Modules.Where(m => m.InitializationMode == InitializationMode.WhenAvailable);
            IEnumerable<ModuleInfo> modulesToLoadTypes = this.ModuleCatalog.CompleteListWithDependencies(whenAvailableModules);
            if (modulesToLoadTypes != null)
            {
                this.LoadModuleTypes(modulesToLoadTypes);
            }
        }

        private void LoadModuleTypes(IEnumerable<ModuleInfo> moduleInfos)
        {
            if (moduleInfos == null)
            {
                return;
            }

            foreach (ModuleInfo moduleInfo in moduleInfos)
            {
                if (moduleInfo.State == ModuleState.NotStarted)
                {
                    if (this.ModuleNeedsRetrieval(moduleInfo))
                    {
                        this.BeginRetrievingModule(moduleInfo);
                    }
                    else
                    {
                        moduleInfo.State = ModuleState.ReadyForInitialization;
                    }
                }
            }

            this.LoadModulesThatAreReadyForLoad();
        }

        /// <summary>
        /// Loads the modules that are not intialized and have their dependencies loaded.
        /// </summary>
        protected void LoadModulesThatAreReadyForLoad()
        {
            bool keepLoading = true;
            while (keepLoading)
            {
                keepLoading = false;
                IEnumerable<ModuleInfo> availableModules = this.ModuleCatalog.Modules.Where(m => m.State == ModuleState.ReadyForInitialization);

                foreach (ModuleInfo moduleInfo in availableModules)
                {
                    if ((moduleInfo.State != ModuleState.Initialized) && (this.AreDependenciesLoaded(moduleInfo)))
                    {
                        moduleInfo.State = ModuleState.Initializing;
                        this.InitializeModule(moduleInfo);
                        keepLoading = true;
                        break;
                    }
                }
            }
        }

        private void BeginRetrievingModule(ModuleInfo moduleInfo)
        {
            ModuleInfo moduleInfoToLoadType = moduleInfo;
            IModuleTypeLoader moduleTypeLoader = this.GetTypeLoaderForModule(moduleInfoToLoadType);
            moduleInfoToLoadType.State = ModuleState.LoadingTypes;

            // Delegate += works differently betweem SL and WPF.
            // We only want to subscribe to each instance once.
            if (!this.subscribedToModuleTypeLoaders.Contains(moduleTypeLoader))
            {
                moduleTypeLoader.ModuleDownloadProgressChanged += this.IModuleTypeLoader_ModuleDownloadProgressChanged;
                moduleTypeLoader.LoadModuleCompleted += this.IModuleTypeLoader_LoadModuleCompleted;
                this.subscribedToModuleTypeLoaders.Add(moduleTypeLoader);
            }

            moduleTypeLoader.LoadModuleType(moduleInfo);
        }

        private void IModuleTypeLoader_ModuleDownloadProgressChanged(object sender, ModuleDownloadProgressChangedEventArgs e)
        {
            this.RaiseModuleDownloadProgressChanged(e);
        }

        private void IModuleTypeLoader_LoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if ((e.ModuleInfo.State != ModuleState.Initializing) && (e.ModuleInfo.State != ModuleState.Initialized))
                {
                    e.ModuleInfo.State = ModuleState.ReadyForInitialization;
                }

                // This callback may call back on the UI thread, but we are not guaranteeing it.
                // If you were to add a custom retriever that retrieved in the background, you
                // would need to consider dispatching to the UI thread.
                this.LoadModulesThatAreReadyForLoad();
            }
            else
            {
                this.RaiseLoadModuleCompleted(e);

                // If the error is not handled then I log it and raise an exception.
                if (!e.IsErrorHandled)
                {
                    this.HandleModuleTypeLoadingError(e.ModuleInfo, e.Error);
                }
            }
        }

        /// <summary>
        /// Handles any exception occurred in the module typeloading process,
        /// logs the error using the <see cref="ILog"/> and throws a <see cref="ModuleTypeLoadingException"/>.
        /// This method can be overridden to provide a different behavior. 
        /// </summary>
        /// <param name="moduleInfo">The module metadata where the error happenened.</param>
        /// <param name="exception">The exception thrown that is the cause of the current error.</param>
        /// <exception cref="ModuleTypeLoadingException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1")]
        protected virtual void HandleModuleTypeLoadingError(ModuleInfo moduleInfo, Exception exception)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException("moduleInfo");

            ModuleTypeLoadingException moduleTypeLoadingException = exception as ModuleTypeLoadingException;

            if (moduleTypeLoadingException == null)
            {
                moduleTypeLoadingException = new ModuleTypeLoadingException(moduleInfo.ModuleName, exception.Message, exception);
            }

            this._loggerFacade.LogError(moduleTypeLoadingException.Message);

            throw moduleTypeLoadingException;
        }

        private bool AreDependenciesLoaded(ModuleInfo moduleInfo)
        {
            IEnumerable<ModuleInfo> requiredModules = this.ModuleCatalog.GetDependentModules(moduleInfo);
            if (requiredModules == null)
            {
                return true;
            }

            int notReadyRequiredModuleCount =
                requiredModules.Count(requiredModule => requiredModule.State != ModuleState.Initialized);

            return notReadyRequiredModuleCount == 0;
        }

        private IModuleTypeLoader GetTypeLoaderForModule(ModuleInfo moduleInfo)
        {
            foreach (IModuleTypeLoader typeLoader in this.ModuleTypeLoaders)
            {
                if (typeLoader.CanLoadModuleType(moduleInfo))
                {
                    return typeLoader;
                }
            }

            throw new ModuleTypeLoaderNotFoundException(moduleInfo.ModuleName, "There is currently no moduleTypeLoader in the ModuleManager that can retrieve the specified module.", null);
        }

        private void InitializeModule(ModuleInfo moduleInfo)
        {
            if (moduleInfo.State == ModuleState.Initializing)
            {
                this._moduleInitializer.Initialize(moduleInfo);
                this.RaiseLoadModuleCompleted(moduleInfo, null);
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the associated <see cref="IModuleTypeLoader"/>s.
        /// </summary>
        /// <param name="disposing">When <see langword="true"/>, it is being called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            foreach (IModuleTypeLoader typeLoader in this.ModuleTypeLoaders)
            {
                var disposableTypeLoader = typeLoader as IDisposable;
                if (disposableTypeLoader != null)
                {
                    disposableTypeLoader.Dispose();
                }
            }
        }

        #endregion
    }
}
