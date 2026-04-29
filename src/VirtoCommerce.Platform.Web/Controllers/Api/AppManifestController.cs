using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using VirtoCommerce.Platform.Core;
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
        private readonly IModuleService _moduleService;

        public AppManifestController(
            IAppManifestService service,
            IModuleService moduleService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _moduleService = moduleService ?? throw new ArgumentNullException(nameof(moduleService));
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

            var response = MapToResponse(descriptor);

            // ETag = SHA1(appId + sorted module ids+versions + sorted user permissions).
            // Lets clients cache the JSON; invalidates whenever the module set, a module
            // version, or the caller's permission set changes.
            var etag = ComputeETag(appId, User);
            var requestEtag = Request.Headers[HeaderNames.IfNoneMatch].ToString();
            if (!string.IsNullOrEmpty(requestEtag) && requestEtag == etag)
            {
                return StatusCode(StatusCodes.Status304NotModified);
            }

            Response.Headers[HeaderNames.ETag] = etag;
            Response.Headers[HeaderNames.CacheControl] = "private, must-revalidate";

            return Ok(response);
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

        private string ComputeETag(string appId, System.Security.Claims.ClaimsPrincipal user)
        {
            var sb = new StringBuilder();
            sb.Append(appId).Append('|');

            foreach (var module in _moduleService.GetInstalledModules().OrderBy(m => m.Id, StringComparer.OrdinalIgnoreCase))
            {
                sb.Append(module.Id).Append('@').Append(module.Version).Append(';');
            }
            sb.Append('|');

            if (user?.Identity?.IsAuthenticated == true)
            {
                var permissions = user
                    .FindAll(PlatformConstants.Security.Claims.PermissionClaimType)
                    .Select(c => c.Value)
                    .OrderBy(p => p, StringComparer.OrdinalIgnoreCase);
                foreach (var p in permissions)
                {
                    sb.Append(p).Append(';');
                }
            }

            // Static SHA1.HashData avoids allocating an instance per request.
            var hash = SHA1.HashData(Encoding.UTF8.GetBytes(sb.ToString()));
            return $"\"{Convert.ToHexString(hash)}\"";
        }
    }

    /// <summary>
    /// Body shape historically posted by VC-Shell to <c>/api/frontend-modules</c>.
    /// </summary>
    public class LegacyFrontendModulesRequest
    {
        public string AppName { get; set; }
    }
}
