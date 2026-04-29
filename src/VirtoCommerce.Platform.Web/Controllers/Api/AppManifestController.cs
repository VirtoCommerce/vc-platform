using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Model.Modularity;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
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
        /// matching <c>If-None-Match</c>. Filters plugins by current user permissions.
        /// </summary>
        [HttpGet("api/apps/{appId}/manifest")]
        [AllowAnonymous]
        public ActionResult<AppManifestResponse> GetManifest(string appId)
        {
            var descriptor = _service.GetManifest(appId, User);
            if (descriptor == null)
            {
                return NotFound();
            }

            // ETag is derived from the descriptor's content-affecting fields,
            // not from out-of-band inputs. It MUST include the per-file
            // cache-busting hashes — otherwise rebuilding a plugin file
            // without bumping its module version produces a fresh
            // `?v={hash}` URL in the body but a stale ETag, and clients
            // sending If-None-Match get a 304 + cached body with old URLs,
            // serving stale plugin code from the browser HTTP cache.
            //
            // User permissions enter the ETag implicitly: the service has
            // already filtered `descriptor.Plugins` by them, so a different
            // permission set yields a different plugin list yields a
            // different ETag.
            var etag = ComputeETag(appId, descriptor);
            var requestEtag = Request.Headers[HeaderNames.IfNoneMatch].ToString();
            if (!string.IsNullOrEmpty(requestEtag) && requestEtag == etag)
            {
                // RFC 7232 §4.1: 304 responses SHOULD echo the ETag so
                // proxies can refresh their cached header set without
                // re-fetching the body.
                Response.Headers[HeaderNames.ETag] = etag;
                Response.Headers[HeaderNames.CacheControl] = "private, must-revalidate";
                return StatusCode(StatusCodes.Status304NotModified);
            }

            Response.Headers[HeaderNames.ETag] = etag;
            Response.Headers[HeaderNames.CacheControl] = "private, must-revalidate";

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

        /// <summary>
        /// Computes a strong ETag covering every field of the response body
        /// that can change between requests: appId + the ordered list of
        /// plugins + each plugin's id, version, entry hash, content-file
        /// hashes, and federation remote coordinates.
        /// </summary>
        /// <remarks>
        /// Plugin order is preserved as-is — it's already deterministic
        /// (topological dependency order from <c>ModuleBootstrapper</c>),
        /// so re-sorting here would just hide intentional ordering changes
        /// from the cache key.
        /// </remarks>
        private static string ComputeETag(string appId, AppManifestDescriptor descriptor)
        {
            var sb = new StringBuilder();
            sb.Append(appId).Append('|');
            sb.Append(descriptor.Version ?? string.Empty).Append('|');

            foreach (var plugin in descriptor.Plugins)
            {
                sb.Append(plugin.Id ?? string.Empty)
                  .Append('@')
                  .Append(plugin.Version ?? string.Empty)
                  .Append('|');

                if (plugin.Entry != null)
                {
                    sb.Append(plugin.Entry.Hash ?? string.Empty);
                }
                sb.Append('|');

                if (plugin.ContentFiles != null)
                {
                    foreach (var file in plugin.ContentFiles)
                    {
                        sb.Append(file?.Hash ?? string.Empty).Append(',');
                    }
                }
                sb.Append('|');

                if (plugin.Remote != null)
                {
                    sb.Append(plugin.Remote.Name ?? string.Empty)
                      .Append('/')
                      .Append(plugin.Remote.Exposed ?? string.Empty);
                }
                sb.Append(';');
            }

            // Static SHA1.HashData avoids allocating an instance per request.
            var hash = SHA1.HashData(Encoding.UTF8.GetBytes(sb.ToString()));
            return $"\"{Convert.ToHexString(hash)}\"";
        }
    }
}
