namespace VirtoCommerce.Platform.Web.Model.Modularity;

/// <summary>
/// One asset belonging to a plugin (script, stylesheet, etc.). Carries the
/// information a client-side loader needs to build the right HTML element
/// with proper cache busting.
/// </summary>
public class ContentFile
{
    /// <summary>
    /// Asset kind. See <see cref="VirtoCommerce.Platform.Core.Modularity.ContentFileTypes"/>
    /// for the canonical values. Lower-case string so the JSON contract is
    /// stable against future C# enum renames.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Absolute URL path served by the platform's static-file middleware,
    /// e.g. <c>/modules/$(VirtoCommerce.Catalog)/dist/app.js</c>.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Cache-busting hash (<c>?v={hash}</c>). Stable across requests as long
    /// as the file's last-write time is unchanged. <c>null</c> when the file
    /// doesn't exist on disk (e.g. a manifest reference that fell back to
    /// convention defaults).
    /// </summary>
    public string Hash { get; set; }
}
