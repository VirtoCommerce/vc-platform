using System;
using System.Linq;
using System.Reflection;
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
        private readonly IModuleCatalog _moduleCatalog;

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleInitializer"/>.
        /// </summary>
        /// <param name="loggerFacade">The logger to use.</param>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="moduleCatalog"></param>
        public ModuleInitializer(
            ILogger<ModuleInitializer> loggerFacade,
            IServiceCollection serviceCollection,
            IConfiguration configuration,
            IHostEnvironment hostingEnvironment,
            IModuleCatalog moduleCatalog)
        {
            _loggerFacade = loggerFacade ?? throw new ArgumentNullException(nameof(loggerFacade));
            _serviceCollection = serviceCollection;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _moduleCatalog = moduleCatalog;
        }

        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="moduleInfo">The module to initialize</param>        
        public void Initialize(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }

            //Do not initialize modules with errors
            if (moduleInfo is ManifestModuleInfo manifestModuleInfo &&
                manifestModuleInfo.Errors.IsNullOrEmpty())
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

                    if (moduleInstance is IHasModuleCatalog hasModuleCatalog)
                    {
                        hasModuleCatalog.ModuleCatalog = _moduleCatalog;
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
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }

            var moduleInstance = moduleInfo.ModuleInstance;

            try
            {
                if (moduleInstance != null)
                {
                    moduleInstance.PostInitialize(appBuilder);
                }
            }
            catch (Exception ex)
            {
                HandleModuleInitializationError(moduleInfo, ex);
            }
        }

        /// <summary>
        /// Handles any exception occurred in the module Initialization process,
        /// logs the error using the <see cref="ILogger"/> and throws a <see cref="ModuleInitializeException"/>.
        /// This method can be overridden to provide a different behavior. 
        /// </summary>
        /// <param name="moduleInfo">The module metadata where the error happened.</param>
        /// <param name="exception">The exception thrown that is the cause of the current error.</param>
        /// <exception cref="ModuleInitializeException"></exception>
        public virtual void HandleModuleInitializationError(ModuleInfo moduleInfo, Exception exception)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

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
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }

            if (!TryResolveModuleTypeFromAssembly(moduleInfo.Assembly, moduleInfo.ModuleType, out var moduleInitializerType) ||
                Activator.CreateInstance(moduleInitializerType) is not IModule module)
            {
                throw new ModuleInitializeException($"Unable to resolve IModule {moduleInfo.ModuleType} from the assembly {moduleInfo.Assembly.FullName}.");
            }

            module.ModuleInfo = moduleInfo as ManifestModuleInfo;

            return module;
        }

        protected virtual bool TryResolveModuleTypeFromAssembly(Assembly moduleAssembly, string moduleType, out Type moduleInitializerType)
        {
            if (moduleAssembly == null)
            {
                throw new ArgumentNullException(nameof(moduleAssembly));
            }

            var moduleInitializerTypes = moduleAssembly.GetTypes().Where(x => typeof(IModule).IsAssignableFrom(x)).ToArray();
            if (!moduleInitializerTypes.Any())
            {
                moduleInitializerType = null;
                return false;
            }

            if (moduleInitializerTypes.Length == 1)
            {
                moduleInitializerType = moduleInitializerTypes.First();
                return true;
            }

            moduleInitializerType = moduleInitializerTypes.FirstOrDefault(x => x.AssemblyQualifiedName?.StartsWith(moduleType) == true);
            return moduleInitializerType != null;
        }
    }
}
