using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Tests.Pipeline;

/// <summary>
/// A startup that participates in no phase at all - every member is left to its default implementation.
/// </summary>
internal sealed class NoHookPlatformStartup : IPlatformStartup
{
}
