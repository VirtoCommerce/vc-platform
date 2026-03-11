namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Determines when IPlatformStartup.Configure() is called in the middleware pipeline.
/// </summary>
public enum PipelinePhase
{
    /// <summary>
    /// Before routing and authentication middleware.
    /// For configuration refresh, custom request preprocessing.
    /// </summary>
    EarlyMiddleware = 0,

    /// <summary>
    /// Within ExecuteSynchronized block, after platform migrations but before IModule.PostInitialize.
    /// For infrastructure requiring database readiness (e.g., Hangfire).
    /// </summary>
    Initialization = 1,

    /// <summary>
    /// After endpoints are mapped and modules are post-initialized.
    /// For middleware needing all endpoints visible (e.g., Swagger).
    /// </summary>
    LateMiddleware = 2
}
