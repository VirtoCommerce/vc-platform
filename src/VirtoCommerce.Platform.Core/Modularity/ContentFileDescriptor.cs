namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Service-layer asset descriptor. Mirrors <c>ContentFile</c> in the web layer.
/// Carries the asset's <see cref="Type"/>, public URL <see cref="Path"/>, and
/// cache-busting <see cref="Hash"/> so client-side loaders can build correct
/// <c>&lt;script&gt;</c> / <c>&lt;link&gt;</c> tags without re-probing the file.
/// </summary>
public class ContentFileDescriptor
{
    public string Type { get; set; }
    public string Path { get; set; }
    public string Hash { get; set; }
}
