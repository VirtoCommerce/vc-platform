using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Web.Model.Security;
using Permissions = VirtoCommerce.Platform.Core.PlatformConstants.Security.Permissions;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/oauthapps")]
    [ApiController]
    [Authorize]
    public class OAuthAppsController : Controller
    {
        private readonly OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication> _manager;

        private readonly ISet<string> _defaultPermissions = new HashSet<string>
        {
            OpenIddictConstants.Permissions.Endpoints.Authorization,
            OpenIddictConstants.Permissions.Endpoints.Logout,
            OpenIddictConstants.Permissions.Endpoints.Token,
            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
            OpenIddictConstants.Permissions.ResponseTypes.Code,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Profile,
        };

        public OAuthAppsController(OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication> manager)
        {
            _manager = manager;
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
            };

            app.Permissions.AddRange(_defaultPermissions);
            return app;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Permissions.SecurityOAuthApplicationsUpdate)]
        public async Task<ActionResult<OpenIddictApplicationDescriptor>> SaveAsync(OpenIddictApplicationDescriptor descriptor)
        {
            descriptor.Permissions.Clear();
            descriptor.Permissions.AddRange(_defaultPermissions);

            var app = await _manager.FindByClientIdAsync(descriptor.ClientId);

            if (app == null)
            {// create
                app = await _manager.CreateAsync(descriptor);
                await _manager.PopulateAsync(descriptor, app);
            }
            else
            {// update
                //prevent changing client secret
                descriptor.ClientSecret = app.ClientSecret;

                await _manager.PopulateAsync(app, descriptor);
                await _manager.UpdateAsync(app);
            }

            descriptor.ClientSecret = app.ClientSecret;
            return descriptor;
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

            var apps = _manager.ListAsync(x => x.OrderBySortInfos(criteria.SortInfos).Skip(criteria.Skip).Take(criteria.Take)).ToEnumerable();

            var appsTasks = apps.Select(async x =>
                {
                    var descriptor = new OpenIddictApplicationDescriptor();
                    await _manager.PopulateAsync(descriptor, x);
                    descriptor.ClientSecret = "";
                    return descriptor;
                }).ToList();

            var result = new OAuthAppSearchResult
            {
                Results = await Task.WhenAll(appsTasks),
                TotalCount = (int)await _manager.CountAsync()
            };

            return result;
        }
    }
}
