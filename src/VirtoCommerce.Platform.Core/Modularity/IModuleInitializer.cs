using System;
using Microsoft.AspNetCore.Builder;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Declares a service which initializes the modules into the application.
    /// </summary>
    [Obsolete("Use ModuleBootstrapper class instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public interface IModuleInitializer
    {
        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="moduleInfo">The module to initialize</param>
        void Initialize(ModuleInfo moduleInfo);

        /// <summary>
        /// Initializes the module during second iteration through all modules.
        /// </summary>
        /// <param name="moduleInfo"></param>
        /// <param name="serviceProvider"></param>
        void PostInitialize(ModuleInfo moduleInfo, IApplicationBuilder serviceProvider);
    }
}
