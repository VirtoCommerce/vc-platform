using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using VirtoCommerce.Platform.Web.Model.Security;
using Permissions = VirtoCommerce.Platform.Core.PlatformConstants.Security.Permissions;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/oauthapps")]
    [ApiController]
    [Authorize]
    public class OAuthAppsController : Controller
    {
        private const string SelfRegisteredAppError = "The application registered itself, so it follows its client metadata document or registration and can't be changed. Delete it to disconnect the client.";

        private readonly OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication> _manager;
        private readonly IEnumerable<IOAuthResourceProvider> _resourceProviders;
        private readonly IOAuthAppRules _rules;
        private readonly Core.Security.AuthorizationOptions _authorizationOptions;
        private readonly ILogger<OAuthAppsController> _logger;

        public OAuthAppsController(
            OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication> manager,
            IEnumerable<IOAuthResourceProvider> resourceProviders,
            IOAuthAppRules rules,
            IOptions<Core.Security.AuthorizationOptions> authorizationOptions,
            ILogger<OAuthAppsController> logger)
        {
            _manager = manager;
            _resourceProviders = resourceProviders;
            _rules = rules;
            _authorizationOptions = authorizationOptions.Value;
            _logger = logger;
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

            _rules.AddDefaultPermissions(app);
            return app;
        }

        /// <summary>
        /// The resources applications can be allowed to request tokens for: those the modules serve, such as MCP
        /// endpoints, and those listed in Authorization:Resources.
        /// </summary>
        [HttpGet]
        [Route("resources")]
        [Authorize(Permissions.SecurityOAuthApplicationsRead)]
        public async Task<ActionResult<OAuthResourceInfo[]>> GetResourcesAsync(CancellationToken cancellationToken)
        {
            var registered = new HashSet<string>(_authorizationOptions.Resources ?? [], StringComparer.Ordinal);
            var result = new Dictionary<string, OAuthResourceInfo>(StringComparer.Ordinal);
            var context = new OAuthResourceContext { HttpContext = HttpContext };

            foreach (var resource in await _resourceProviders.GetAllResourcesAsync(context, _logger, cancellationToken))
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
            var app = await FindStoredByClientIdAsync(DecodeRouteValue(clientId));
            if (app == null)
            {
                return NotFound();
            }

            return await ToDescriptorAsync(app);
        }

        /// <summary>
        /// Creates or updates the application with the client id. The application keeps the permissions it is saved
        /// with; the endpoint permissions follow from its grant types, and an application that names no grant type
        /// gets the platform defaults. The client secret can't be changed and is never returned. The application of a
        /// self-registered client can't be changed: it follows its client metadata document or registration.
        /// </summary>
        [HttpPost]
        [Route("")]
        [Authorize(Permissions.SecurityOAuthApplicationsUpdate)]
        public async Task<ActionResult<OpenIddictApplicationDescriptor>> SaveAsync(OpenIddictApplicationDescriptor descriptor)
        {
            var app = string.IsNullOrWhiteSpace(descriptor.ClientId) ? null : await FindStoredByClientIdAsync(descriptor.ClientId);

            if (await _manager.IsSelfRegisteredAsync(app))
            {
                return BadRequest(new SecurityResult { Errors = [SelfRegisteredAppError] });
            }

            descriptor.Properties.Remove(SelfRegisteredClientConstants.RegistrationProperty);

            var stored = app == null ? null : await ToDescriptorAsync(app);

            _rules.Normalize(descriptor, stored);

            if (stored != null)
            {
                // the secret can't be changed: the stored (hashed) one is kept, and the search never returns it
                descriptor.ClientSecret = null;
            }

            var errors = _rules.Validate(descriptor, stored);
            if (errors.Count > 0)
            {
                return BadRequest(new SecurityResult { Errors = errors });
            }

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

            var apps = _manager.ListAsync(x => Filter(x, criteria).OrderBySortInfos(criteria.SortInfos).Skip(criteria.Skip).Take(criteria.Take));

            var appsTasks = await apps.Select(x => ToDescriptorAsync(x)).ToListAsync();

            var result = new OAuthAppSearchResult
            {
                Results = await Task.WhenAll(appsTasks),
                TotalCount = (int)await _manager.CountAsync(x => Filter(x, criteria))
            };

            return result;
        }

        /// <summary>
        /// Without resolving a self-registered client, so it opens even when its document is gone or its way of
        /// registration is turned off.
        /// </summary>
        protected virtual async Task<VirtoOpenIddictEntityFrameworkCoreApplication> FindStoredByClientIdAsync(string clientId)
        {
            return _manager is VirtoOpenIddictApplicationManager manager
                ? await manager.FindStoredByClientIdAsync(clientId)
                : await _manager.FindByClientIdAsync(clientId);
        }

        /// <summary>
        /// ASP.NET Core decodes a route value except for an encoded slash, which a client id that is a URL has.
        /// </summary>
        protected static string DecodeRouteValue(string value)
        {
            return value?.Replace("%2F", "/", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// OpenIddict stores the properties as a JSON object, so the registration property is looked for as a name in it.
        /// </summary>
        protected static IQueryable<VirtoOpenIddictEntityFrameworkCoreApplication> Filter(IQueryable<VirtoOpenIddictEntityFrameworkCoreApplication> query,
            OAuthAppSearchCriteria criteria)
        {
            var registration = $"\"{SelfRegisteredClientConstants.RegistrationProperty}\":";

            return criteria.SelfRegistered switch
            {
                true => query.Where(x => x.Properties != null && x.Properties.Contains(registration)),
                false => query.Where(x => x.Properties == null || !x.Properties.Contains(registration)),
                _ => query,
            };
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
