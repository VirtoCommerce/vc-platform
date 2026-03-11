namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Well-known priority constants for IPlatformStartup ordering.
/// Lower values execute first.
/// </summary>
public static class StartupPriority
{
    /// <summary>Configuration sources load first.</summary>
    public const int ConfigurationSource = -1000;

    /// <summary>Infrastructure services like logging and caching.</summary>
    public const int Infrastructure = -500;

    /// <summary>Default priority for module startup types.</summary>
    public const int Default = 0;

    /// <summary>Services depending on other modules.</summary>
    public const int Late = 500;
}
