namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Declares a service which initializes the modules into the application.
    /// </summary>
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
        void PostInitialize(ModuleInfo moduleInfo);
    }
}
