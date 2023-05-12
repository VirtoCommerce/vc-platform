using System;

namespace VirtoCommerce.Platform.Data;

public class DataOptions
{
    /// <summary>
    /// The command timeout set for EF Core migrations.
    /// </summary>
    public TimeSpan? MigrationsTimeout { get; set; }
}
