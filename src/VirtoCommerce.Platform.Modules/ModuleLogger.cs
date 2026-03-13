using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace VirtoCommerce.Platform.Modules;

/// <summary>
/// Static logger factory for early module loading (before DI).
/// Must be initialized by the host (e.g., Program.Main) before use.
/// Falls back to NullLoggerFactory if not initialized.
/// </summary>
public static class ModuleLogger
{
    private static ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;

    /// <summary>
    /// Set the logger factory. Call once from Program.Main before module loading.
    /// </summary>
    public static void Initialize(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory ?? NullLoggerFactory.Instance;
    }

    public static ILogger CreateLogger(Type type) => _loggerFactory.CreateLogger(type);

    /// <summary>
    /// Reset to NullLoggerFactory (for testing).
    /// </summary>
    public static void Reset() => _loggerFactory = NullLoggerFactory.Instance;
}
