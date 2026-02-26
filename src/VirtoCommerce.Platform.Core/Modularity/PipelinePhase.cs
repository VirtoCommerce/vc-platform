namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Determines when an <see cref="IPlatformStartup.Configure"/> method is invoked
    /// within the Startup.Configure pipeline.
    /// </summary>
    public enum PipelinePhase
    {
        /// <summary>
        /// Before routing and authentication middleware.
        /// Use for configuration refresh middleware, custom request preprocessing, etc.
        /// </summary>
        EarlyMiddleware = 0,

        /// <summary>
        /// Within the ExecuteSynchronized block, after platform migrations
        /// but before IModule.PostInitialize.
        /// Use for infrastructure that requires the database to be ready (e.g., Hangfire).
        /// </summary>
        Initialization = 1,

        /// <summary>
        /// After endpoints are mapped and modules are post-initialized.
        /// Use for middleware that needs to see all endpoints (e.g., Swagger).
        /// </summary>
        LateMiddleware = 2
    }
}
