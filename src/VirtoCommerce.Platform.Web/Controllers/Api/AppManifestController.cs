using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Model.Modularity;

namespace VirtoCommerce.Platform.Web.Controllers.Api;

/// <summary>
/// Backoffice modularity manifest endpoint. Returns the topologically
/// ordered, permission-filtered plugin set for a host app
/// (legacy AngularJS, VC-Shell, or any custom standalone SPA).
///
/// See <c>docs/developer-guide/backoffice-modularity-framework.md</c> for the
/// full architecture, plugin authoring guide, and migration plan.
/// </summary>
[ApiController]
public class AppManifestController : ControllerBase
{
    private readonly IAppManifestService _service;

    public AppManifestController(IAppManifestService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Returns the host app's plugin manifest. Cacheable — responds 304 on
    /// matching <c>If-None-Match</c>. The descriptor is built and hashed
    /// once at the service layer (<see cref="IAppManifestService"/>) and
    /// cached there for the process lifetime, so 304 cache-hits short-circuit
    /// without touching the filesystem.
    /// </summary>
    [HttpGet("api/apps/{appId}/manifest")]
    [AllowAnonymous]
    public ActionResult<AppManifestResponse> GetManifest(string appId)
    {
        var descriptor = _service.GetManifest(appId);

        if (descriptor == null)
        {
            return NotFound();
        }

        // The service computed `descriptor.Hash` during build (covers AppId,
        // Version, the ordered Plugins list with per-file mtime hashes, and
        // federation remote coordinates) — wrap it in quotes per RFC 7232
        // strong-ETag syntax and let HTTP do the conditional GET.
        var etag = $"\"{descriptor.Hash}\"";
        Response.Headers[HeaderNames.ETag] = etag;
        Response.Headers[HeaderNames.CacheControl] = "private, must-revalidate";

        var requestEtag = Request.Headers[HeaderNames.IfNoneMatch].ToString();
        if (!string.IsNullOrEmpty(requestEtag) && requestEtag == etag)
        {
            // RFC 7232 §4.1: 304 responses SHOULD echo the ETag so
            // proxies can refresh their cached header set without
            // re-fetching the body. (Already set above.)
            return StatusCode(StatusCodes.Status304NotModified);
        }

        return Ok(MapToResponse(descriptor));
    }

    private static AppManifestResponse MapToResponse(AppManifestDescriptor descriptor) => new()
    {
        AppId = descriptor.AppId,
        Version = descriptor.Version,
        Title = descriptor.Title,
        Plugins = descriptor.Plugins.Select(p => new PluginEntry
        {
            Id = p.Id,
            Version = p.Version,
            Entry = MapFile(p.Entry),
            ContentFiles = p.ContentFiles.Select(MapFile).ToList(),
            Remote = p.Remote == null ? null : new PluginRemote
            {
                Name = p.Remote.Name,
                Exposed = p.Remote.Exposed,
            },
        }).ToList(),
    };

    private static ContentFile MapFile(ContentFileDescriptor source) => source == null ? null : new ContentFile
    {
        Type = source.Type,
        Path = source.Path,
        Hash = source.Hash,
    };
}
