using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Search;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/oauthapps")]
    [ApiController]
    [Authorize]
    public class OAuthAppsController : Controller
    {
        private readonly OpenIddictApplicationManager<OpenIddictApplication> _manager;

        private readonly ISet<string> _defaultPermissions = new HashSet<string>
        {
            OpenIddictConstants.Permissions.Endpoints.Authorization,
            OpenIddictConstants.Permissions.Endpoints.Token,
            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            OpenIddictConstants.Permissions.GrantTypes.ClientCredentials
        };

        public OAuthAppsController(OpenIddictApplicationManager<OpenIddictApplication> manager)
        {
            _manager = manager;
        }

        [HttpGet]
        [Route("new")]
        public ActionResult<OpenIddictApplicationDescriptor> New()
        {
            var app = new OpenIddictApplicationDescriptor
            {
                DisplayName = "New application",
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = Guid.NewGuid().ToString(),
                Type = OpenIddictConstants.ClientTypes.Confidential
            };

            app.Permissions.AddRange(_defaultPermissions);
            return app;
        }

        [HttpPost]
        [Route("")]
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
        public async Task<ActionResult> DeleteAsync([FromQuery] string[] clientIds)
        {
            var apps = await _manager.ListAsync(x =>
                x.Where(y => clientIds.Contains(y.ClientId)));

            foreach (var app in apps)
            {
                await _manager.DeleteAsync(app);
            }

            return Ok();
        }

        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<OAuthAppSearchResult>> SearchAsync(OAuthAppSearchCriteria criteria)
        {
            if (criteria.Sort.IsNullOrEmpty())
            {
                criteria.Sort = "DisplayName:ASC";
            }

            var apps = await _manager.ListAsync(x =>
                x.OrderBySortInfos(criteria.SortInfos).Skip(criteria.Skip).Take(criteria.Take));

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
