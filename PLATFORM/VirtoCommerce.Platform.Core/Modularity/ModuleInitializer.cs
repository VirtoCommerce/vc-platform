using System;
using System.Globalization;
using Common.Logging;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Implements the <see cref="IModuleInitializer"/> interface. Handles loading of a module based on a type.
    /// </summary>
    public class ModuleInitializer : IModuleInitializer
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly ILog _loggerFacade;
        private readonly IModuleInitializerOptions _options;

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleInitializer"/>.
        /// </summary>
        /// <param name="serviceLocator">The container that will be used to resolve the modules by specifying its type.</param>
        /// <param name="loggerFacade">The logger to use.</param>
        /// <param name="options">The module initializer options.</param>
        public ModuleInitializer(IServiceLocator serviceLocator, ILog loggerFacade, IModuleInitializerOptions options)
        {
            if (serviceLocator == null)
            {
                throw new ArgumentNullException("serviceLocator");
            }

            if (loggerFacade == null)
            {
                throw new ArgumentNullException("loggerFacade");
            }

            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            _serviceLocator = serviceLocator;
            _loggerFacade = loggerFacade;
            _options = options;
        }

        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="moduleInfo">The module to initialize</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Catches Exception to handle any exception thrown during the initialization process with the HandleModuleInitializationError method.")]
        public void Initialize(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException("moduleInfo");

            IModule moduleInstance = null;
            try
            {
                moduleInstance = CreateModule(moduleInfo);
                moduleInstance.SetupDatabase(_options.SampleDataLevel);
                moduleInstance.Initialize();
                moduleInfo.ModuleInstance = moduleInstance;
            }
            catch (Exception ex)
            {
                HandleModuleInitializationError(
                    moduleInfo,
                    moduleInstance != null ? moduleInstance.GetType().Assembly.FullName : null,
                    ex);
            }
        }

        /// <summary>
        /// Handles any exception occurred in the module Initialization process,
        /// logs the error using the <see cref="ILog"/> and throws a <see cref="ModuleInitializeException"/>.
        /// This method can be overridden to provide a different behavior. 
        /// </summary>
        /// <param name="moduleInfo">The module metadata where the error happenened.</param>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="exception">The exception thrown that is the cause of the current error.</param>
        /// <exception cref="ModuleInitializeException"></exception>
        public virtual void HandleModuleInitializationError(ModuleInfo moduleInfo, string assemblyName, Exception exception)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException("moduleInfo");
            if (exception == null)
                throw new ArgumentNullException("exception");

            Exception moduleException;

            if (exception is ModuleInitializeException)
            {
                moduleException = exception;
            }
            else
            {
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, assemblyName, exception.Message, exception);
                }
                else
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, exception.Message, exception);
                }
            }

            _loggerFacade.Error(moduleException.ToString());

            throw moduleException;
        }

        /// <summary>
        /// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
        /// </summary>
        /// <param name="moduleInfo">The module to create.</param>
        /// <returns>A new instance of the module specified by <paramref name="moduleInfo"/>.</returns>
        protected virtual IModule CreateModule(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException("moduleInfo");
            return CreateModule(moduleInfo.ModuleType);
        }

        /// <summary>
        /// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The type name to resolve. This type must implement <see cref="IModule"/>.</param>
        /// <returns>A new instance of <paramref name="typeName"/>.</returns>
        protected virtual IModule CreateModule(string typeName)
        {
            Type moduleType = Type.GetType(typeName);
            if (moduleType == null)
            {
                throw new ModuleInitializeException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.FailedToGetType, typeName));
            }

            return (IModule)_serviceLocator.GetInstance(moduleType);
        }
    }
}
