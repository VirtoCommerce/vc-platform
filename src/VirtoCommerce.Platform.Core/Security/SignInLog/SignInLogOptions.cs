namespace VirtoCommerce.Platform.Core.Security.SignInLog;

/// <summary>
/// Buffer sizing for the sign-in audit writer. Bound from the "SignInLog" configuration section
/// so a high-traffic store can tune it without a recompile.
/// </summary>
public class SignInLogOptions
{
    /// <summary>Maximum records held in memory before the writer starts dropping.</summary>
    public int BufferCapacity { get; set; } = 10_000;

    /// <summary>Records written per database round trip.</summary>
    public int BatchSize { get; set; } = 200;

    /// <summary>How often the background loop drains the buffer.</summary>
    public int FlushIntervalSeconds { get; set; } = 5;
}
