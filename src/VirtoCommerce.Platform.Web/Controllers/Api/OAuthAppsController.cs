using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Web.Model.Security;
using VirtoCommerce.Platform.Security.OpenIddict;
using Permissions = VirtoCommerce.Platform.Core.PlatformConstants.Security.Permissions;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/oauthapps")]
    [ApiController]
    [Authorize]
    public class OAuthAppsController : Controller
    {
        private readonly OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication> _manager;
        private readonly IEnumerable<IOAuthResourceProvider> _resourceProviders;
        private readonly Core.Security.AuthorizationOptions _authorizationOptions;

        public OAuthAppsController(
            OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication> manager,
            IEnumerable<IOAuthResourceProvider> resourceProviders,
            IOptions<Core.Security.AuthorizationOptions> authorizationOptions)
        {
            _manager = manager;
            _resourceProviders = resourceProviders;
            _authorizationOptions = authorizationOptions.Value;
        }

        [HttpGet]
        [Route("new")]
        [Authorize(Permissions.SecurityOAuthApplicationsCreate)]
        public ActionResult<OpenIddictApplicationDescriptor> New()
        {
            var app = new OpenIddictApplicationDescriptor
            {
                DisplayName = "New application",
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
            };

            OAuthAppRules.AddDefaultPermissions(app);
            return app;
        }

        /// <summary>
        /// The resources applications can be allowed to request tokens for: those the modules serve, such as MCP
        /// endpoints, and those listed in Authorization:Resources.
        /// </summary>
        [HttpGet]
        [Route("resources")]
        [Authorize(Permissions.SecurityOAuthApplicationsRead)]
        public async Task<ActionResult<OAuthResourceInfo[]>> GetResourcesAsync()
        {
            var registered = new HashSet<string>(_authorizationOptions.Resources ?? [], StringComparer.Ordinal);
            var result = new Dictionary<string, OAuthResourceInfo>(StringComparer.Ordinal);

            foreach (var provider in _resourceProviders)
            {
                foreach (var resource in await provider.GetResourcesAsync(HttpContext) ?? [])
                {
                    if (!string.IsNullOrEmpty(resource?.Uri))
                    {
                        result.TryAdd(resource.Uri, new OAuthResourceInfo
                        {
                            Uri = resource.Uri,
                            Title = resource.Title,
                            Kind = resource.Kind,
                            Description = resource.Description,
                            IsRegistered = registered.Contains(resource.Uri),
                        });
                    }
                }
            }

            foreach (var uri in registered.Where(x => !string.IsNullOrEmpty(x)))
            {
                result.TryAdd(uri, new OAuthResourceInfo { Uri = uri, IsRegistered = true });
            }

            return Ok(result.Values.ToArray());
        }

        [HttpGet]
        [Route("{clientId}")]
        [Authorize(Permissions.SecurityOAuthApplicationsRead)]
        public async Task<ActionResult<OpenIddictApplicationDescriptor>> GetAsync([FromRoute] string clientId)
        {
            var app = await _manager.FindByClientIdAsync(clientId);
            if (app == null)
            {
                return NotFound();
            }

            return await ToDescriptorAsync(app);
        }

        /// <summary>
        /// Creates or updates the application with the client id. The application keeps the permissions it is saved
        /// with; the endpoint permissions follow from its grant types, and an application that names no grant type
        /// gets the platform defaults. The client secret can't be changed and is never returned.
        /// </summary>
        [HttpPost]
        [Route("")]
        [Authorize(Permissions.SecurityOAuthApplicationsUpdate)]
        public async Task<ActionResult<OpenIddictApplicationDescriptor>> SaveAsync(OpenIddictApplicationDescriptor descriptor)
        {
            var app = string.IsNullOrWhiteSpace(descriptor.ClientId) ? null : await _manager.FindByClientIdAsync(descriptor.ClientId);
            var stored = app == null ? null : await ToDescriptorAsync(app);

            descriptor.ClientType ??= stored?.ClientType ?? (string.IsNullOrEmpty(descriptor.ClientSecret)
                ? OpenIddictConstants.ClientTypes.Public
                : OpenIddictConstants.ClientTypes.Confidential);

            if (stored != null)
            {
                // the secret can't be changed: the stored (hashed) one is kept, and the search never returns it
                descriptor.ClientSecret = null;
            }

            var errors = OAuthAppRules.Validate(descriptor, stored);
            if (errors.Count > 0)
            {
                return BadRequest(new SecurityResult { Errors = errors });
            }

            OAuthAppRules.NormalizePermissions(descriptor);

            try
            {
                if (app == null)
                {
                    app = await _manager.CreateAsync(descriptor);
                }
                else
                {
                    descriptor.ClientSecret = app.ClientSecret;
                    await _manager.PopulateAsync(app, descriptor);
                    await _manager.UpdateAsync(app);
                }
            }
            catch (OpenIddictExceptions.ValidationException ex)
            {
                return BadRequest(new SecurityResult { Errors = ex.Results.Select(x => x.ErrorMessage).ToList() });
            }

            return await ToDescriptorAsync(app);
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Permissions.SecurityOAuthApplicationsDelete)]
        public async Task<ActionResult> DeleteAsync([FromQuery] string[] clientIds)
        {
            var apps = await _manager.ListAsync(x => x.Where(y => clientIds.Contains(y.ClientId))).ToListAsync();

            foreach (var app in apps)
            {
                await _manager.DeleteAsync(app);
            }

            return Ok();
        }

        [HttpPost]
        [Route("search")]
        [Authorize(Permissions.SecurityOAuthApplicationsRead)]
        public async Task<ActionResult<OAuthAppSearchResult>> SearchAsync(OAuthAppSearchCriteria criteria)
        {
            if (criteria.Sort.IsNullOrEmpty())
            {
                criteria.Sort = "DisplayName:ASC";
            }

            var apps = _manager.ListAsync(x => x.OrderBySortInfos(criteria.SortInfos).Skip(criteria.Skip).Take(criteria.Take));

            var appsTasks = await apps.Select(x => ToDescriptorAsync(x)).ToListAsync();

            var result = new OAuthAppSearchResult
            {
                Results = await Task.WhenAll(appsTasks),
                TotalCount = (int)await _manager.CountAsync()
            };

            return result;
        }

        private async Task<OpenIddictApplicationDescriptor> ToDescriptorAsync(VirtoOpenIddictEntityFrameworkCoreApplication app)
        {
            var descriptor = new OpenIddictApplicationDescriptor();
            await _manager.PopulateAsync(descriptor, app);
            descriptor.ClientSecret = "";
            return descriptor;
        }
    }
}
