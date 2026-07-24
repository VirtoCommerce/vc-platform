using System;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Thrown when background-job work is requested but no background-job engine module is installed.
/// Carries an actionable message telling the operator how to install an engine via the Virto Commerce CLI.
/// </summary>
public class BackgroundJobEngineNotInstalledException : InvalidOperationException
{
    /// <summary>
    /// Id of the module that provides the default (Hangfire) background-job engine.
    /// </summary>
    public const string DefaultEngineModuleId = "VirtoCommerce.BackgroundJobs";

    /// <summary>
    /// Default actionable message shown to operators.
    /// </summary>
    public const string DefaultMessage =
        "No background-job engine is installed, so background processing is unavailable. " +
        "Install a background-job engine module via the Virto Commerce CLI, e.g. " +
        "`vc-build install -Module " + DefaultEngineModuleId + "`, then restart the platform.";

    public BackgroundJobEngineNotInstalledException()
        : base(DefaultMessage)
    {
    }

    public BackgroundJobEngineNotInstalledException(string message)
        : base(message)
    {
    }

    public BackgroundJobEngineNotInstalledException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
