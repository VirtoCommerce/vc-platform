namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// The sign-in page of OAuth authorization requests that arrive on one host.
/// </summary>
public class OAuthHostLoginPath
{
    /// <summary>
    /// The host, without the scheme and the port, such as admin.example.com. Matched ignoring case.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// The local path of the front-end's sign-in page on the host, such as /oauth/authorize. Null or empty selects
    /// the built-in Platform login page.
    /// </summary>
    public string Path { get; set; }
}
