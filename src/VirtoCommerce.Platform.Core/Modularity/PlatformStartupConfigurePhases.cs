using System;

namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Determines when IPlatformStartup.Configure() is called in the middleware pipeline.
/// </summary>
[Flags]
public enum PlatformStartupConfigurePhases
{
    Undefined = 0,

    /// <summary>
    /// Before routing and authentication middleware.
    /// For configuration refresh, custom request preprocessing.
    /// </summary>
    EarlyMiddleware = 1,

    /// <summary>
    /// Within ExecuteSynchronized block, after platform migrations but before IModule.PostInitialize.
    /// For infrastructure requiring database readiness (e.g., Hangfire).
    /// </summary>
    ExecuteSynchronized = 2,

    /// <summary>
    /// After endpoints are mapped and modules are post-initialized.
    /// For middleware needing all endpoints visible (e.g., Swagger).
    /// </summary>
    LateMiddleware = 4
}
