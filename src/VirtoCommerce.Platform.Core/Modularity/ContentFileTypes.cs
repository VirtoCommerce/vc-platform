namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// File-asset kinds the modularity framework recognises. The web layer's
/// <c>ContentFile.Type</c> property carries these values verbatim — they are
/// kept as plain strings so the JSON contract stays stable across future
/// C# renames.
/// </summary>
public static class ContentFileTypes
{
    public const string Script = "script";
    public const string Style = "style";
}
