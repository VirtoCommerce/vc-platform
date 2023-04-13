using System;

namespace VirtoCommerce.Platform.Data;

public class DataOptions
{
    /// <summary>
    /// The command timeout set for migrations that upgrade legacy databases created in VC v2.
    /// </summary>
    public TimeSpan? PlatformV2UpdateTimeout { get; set; }
}
