namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Defines the contract for the modules deployed in the application.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Allows module to configure database.
        /// This method is called before Initialize().
        /// </summary>
        void SetupDatabase();

        /// <summary>
        /// Notifies the module that it has been initialized.
        /// </summary>
        void Initialize();

        /// <summary>
        /// This method is called after all modules have been initialized with Initialize().
        /// </summary>
        void PostInitialize();

        /// <summary>
        /// This method is called before uninstalling the module.
        /// </summary>
        void Uninstall();
    }
}
