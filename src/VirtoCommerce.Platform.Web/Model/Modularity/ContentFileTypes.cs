namespace VirtoCommerce.Platform.Web.Model.Modularity;

/// <summary>
/// File-asset kinds the modularity framework knows how to ship to a host.
/// Mapped 1:1 to HTML elements by the loader: <c>script</c> → <c>&lt;script&gt;</c>,
/// <c>style</c> → <c>&lt;link rel="stylesheet"&gt;</c>.
/// </summary>
public static class ContentFileTypes
{
    public const string Script = "script";
    public const string Style = "style";
}
