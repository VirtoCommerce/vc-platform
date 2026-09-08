using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Tests.Modularity;

/// <summary>
/// A startup that participates in no phase at all - every member is left to its default implementation.
/// </summary>
public class NoHookPlatformStartup : IPlatformStartup
{
}
