using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class OAuth2SecurityRequirementsFilter : IOperationFilter
    {
        [CLSCompliant(false)]
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (apiDescription.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any())
            {
                operation.security = null;
                return;
            }

            var permissionAttributes = apiDescription.GetControllerAndActionAttributes<CheckPermissionAttribute>()
                    .Concat(apiDescription.ActionDescriptor.Configuration.Filters.Select(f => f.Instance).OfType<CheckPermissionAttribute>());

            var permissions = permissionAttributes.SelectMany(x => x.Permissions).ToList();
            if (permissions.Count == 0)
            {
                return;
            }

            var securityRequirements = new Dictionary<string, IEnumerable<string>>
            {
                {"OAuth2", permissions},
                {"apiKey", permissions}
            };

            if (operation.security == null)
            {
                operation.security = new List<IDictionary<string, IEnumerable<string>>>(1);
            }

            operation.security.Add(securityRequirements);
        }
    }
}
