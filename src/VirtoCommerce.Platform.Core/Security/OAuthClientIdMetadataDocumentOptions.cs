using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Clients identified by the HTTPS URL of their metadata document (OAuth Client ID Metadata Document).
/// </summary>
public class OAuthClientIdMetadataDocumentOptions
{
    private const int DefaultMaxDocumentSize = 5120;

    /// <summary>
    /// Accept an HTTPS URL as the client id. By default, true.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The hosts the documents may be fetched from, such as claude.ai, or *.example.com for the subdomains of
    /// example.com. Empty, the default, allows any public host.
    /// </summary>
    public IList<string> AllowedHosts { get; set; } = [];

    /// <summary>
    /// The longest a document is kept before it is fetched again, whatever its Cache-Control header says. By default, 1 hour.
    /// </summary>
    public TimeSpan MaxCacheLifetime { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// The shortest a document is kept, even when its Cache-Control header says less. By default, 5 minutes.
    /// </summary>
    public TimeSpan MinCacheLifetime { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// The time to fetch a document. By default, 5 seconds.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The largest document, in bytes. By default, 5 KB.
    /// </summary>
    public int MaxDocumentSize { get; set; } = DefaultMaxDocumentSize;

    /// <summary>
    /// Fetch documents from this computer too (localhost, 127.0.0.1, [::1]), to try a client during development.
    /// Honored only in the Development environment; private network addresses stay refused. By default, false.
    /// </summary>
    public bool AllowLoopbackInDevelopment { get; set; }
}
