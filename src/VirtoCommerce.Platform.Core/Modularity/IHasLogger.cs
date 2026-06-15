using Microsoft.Extensions.Logging;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Used to set an <see cref="ILogger"/> for modules and platform startups during initialization.
    /// In order to log messages from a module or an <see cref="IPlatformStartup"/> you have to implement this
    /// interface for your class - the platform assigns a logger scoped to the implementing type.
    /// </summary>
    public interface IHasLogger
    {
        ILogger Logger { get; set; }
    }
}
