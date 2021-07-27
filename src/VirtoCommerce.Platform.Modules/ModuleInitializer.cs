using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Modules
{
    /// <summary>
    /// Implements the <see cref="IModuleInitializer"/> interface. Handles loading of a module based on a type.
    /// </summary>
    public class ModuleInitializer : IModuleInitializer
    {
        private readonly ILogger<ModuleInitializer> _loggerFacade;
        private readonly IServiceCollection _serviceCollection;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleInitializer"/>.
        /// </summary>
        /// <param name="loggerFacade">The logger to use.</param>
        public ModuleInitializer(
            ILogger<ModuleInitializer> loggerFacade,
            IServiceCollection serviceCollection,
            IConfiguration configuration,
            IHostEnvironment hostingEnvironment)
        {
            _loggerFacade = loggerFacade ?? throw new ArgumentNullException("loggerFacade");
            _serviceCollection = serviceCollection;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="moduleInfo">The module to initialize</param>        
        public void Initialize(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException("moduleInfo");

            var manifestModuleInfo = moduleInfo as ManifestModuleInfo;
            //Do not initialize modules with errors
            if (manifestModuleInfo.Errors.IsNullOrEmpty())
            {
                try
                {
                    var moduleInstance = CreateModule(moduleInfo);
                    moduleInfo.ModuleInstance = moduleInstance;

                    if (moduleInstance is IHasConfiguration configurableModule)
                    {
                        configurableModule.Configuration = _configuration;
                    }
                    if (moduleInstance is IHasHostEnvironment hasHostEnvironment)
                    {
                        hasHostEnvironment.HostEnvironment = _hostingEnvironment;
                    }
                    moduleInstance.Initialize(_serviceCollection);
                    moduleInfo.State = ModuleState.Initialized;
                }
                catch (Exception ex)
                {
                    HandleModuleInitializationError(moduleInfo, ex);
                }
            }
        }

        public void PostInitialize(ModuleInfo moduleInfo, IApplicationBuilder appBuilder)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException("moduleInfo");

            var moduleInstance = moduleInfo.ModuleInstance;

            try
            {
                moduleInstance.PostInitialize(appBuilder);
            }
            catch (Exception ex)
            {
                HandleModuleInitializationError(moduleInfo, ex);
            }
        }

        /// <summary>
        /// Handles any exception occurred in the module Initialization process,
        /// logs the error using the <see cref="ILog"/> and throws a <see cref="ModuleInitializeException"/>.
        /// This method can be overridden to provide a different behavior. 
        /// </summary>
        /// <param name="moduleInfo">The module metadata where the error happenened.</param>
        /// <param name="exception">The exception thrown that is the cause of the current error.</param>
        /// <exception cref="ModuleInitializeException"></exception>
        public virtual void HandleModuleInitializationError(ModuleInfo moduleInfo, Exception exception)
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
                if (moduleInfo.ModuleInstance != null)
                {
                    var assemblyName = moduleInfo.ModuleInstance.GetType().Assembly.FullName;
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, assemblyName, exception.Message, exception);
                }
                else
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, exception.Message, exception);
                }
            }
            if (moduleInfo is ManifestModuleInfo manifestModule)
            {
                manifestModule.Errors.Add(exception.ToString());
            }
            _loggerFacade.LogError(moduleException.ToString());
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

            IModule result = null;
            var moduleInitializerType = moduleInfo.Assembly.GetTypes().FirstOrDefault(x => typeof(IModule).IsAssignableFrom(x));
            if (moduleInitializerType != null && moduleInitializerType != typeof(IModule))
            {
                result = (IModule)Activator.CreateInstance(moduleInitializerType);
            }
            if (result == null)
            {
                throw new ModuleInitializeException($"Unable to retrieve the module type {moduleInitializerType} from the loaded assemblies.You may need to specify a more fully - qualified type name");
            }
            result.ModuleInfo = moduleInfo as ManifestModuleInfo;
            return result;
        }
    }
}
