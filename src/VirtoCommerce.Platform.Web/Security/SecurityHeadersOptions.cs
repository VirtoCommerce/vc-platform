namespace VirtoCommerce.Platform.Web.Security;

public class SecurityHeadersOptions
{
    /// <summary>
    /// Allows to configure X-Frame-Options header.
    /// Allowed values: Deny - default, SameOrigin, or custom uri.
    /// </summary>
    public string FrameOptions { get; set; } = "Deny";

    /// <summary>
    /// Allows to configure values for FrameAncestors in Content-Security-Header header.
    /// Allowed values: None - default, Self, or custom uri.
    /// </summary>
    public string FrameAncestors { get; set; } = "None";
}
