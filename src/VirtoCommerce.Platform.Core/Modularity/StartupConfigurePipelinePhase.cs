namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Determines when IPlatformStartup.Configure() is called in the middleware pipeline.
/// </summary>
public enum StartupConfigurePipelinePhase
{
    Undefined,

    /// <summary>
    /// Before routing and authentication middleware.
    /// For configuration refresh, custom request preprocessing.
    /// </summary>
    EarlyMiddleware,

    /// <summary>
    /// Within ExecuteSynchronized block, after platform migrations but before IModule.PostInitialize.
    /// For infrastructure requiring database readiness (e.g., Hangfire).
    /// </summary>
    Initialization,

    /// <summary>
    /// After endpoints are mapped and modules are post-initialized.
    /// For middleware needing all endpoints visible (e.g., Swagger).
    /// </summary>
    LateMiddleware,
}
