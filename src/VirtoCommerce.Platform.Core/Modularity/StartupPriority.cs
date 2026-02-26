namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Well-known priority values for <see cref="IPlatformStartup"/> ordering.
    /// Lower values execute first.
    /// </summary>
    public static class StartupPriority
    {
        /// <summary>Configuration sources should load first (e.g., Azure App Configuration).</summary>
        public const int ConfigurationSource = -1000;

        /// <summary>Infrastructure services like logging and caching (e.g., Serilog).</summary>
        public const int Infrastructure = -500;

        /// <summary>Default priority for module startup types.</summary>
        public const int Default = 0;

        /// <summary>Services that depend on other modules being registered (e.g., Swagger).</summary>
        public const int Late = 500;
    }
}
