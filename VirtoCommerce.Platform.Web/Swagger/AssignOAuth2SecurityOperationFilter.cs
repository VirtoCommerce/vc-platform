using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class AssignOAuth2SecurityOperationFilter : IOperationFilter
    {
        [CLSCompliant(false)]
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (apiDescription.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any())
            {
                operation.security = null;
                return;
            }

            var permissionAttributes = apiDescription.ActionDescriptor
                .GetFilterPipeline().Select(x => x.Instance).OfType<CheckPermissionAttribute>();

            var permissions = permissionAttributes.SelectMany(x => x.Permissions).Distinct().ToList();
            if (permissions.Count == 0)
            {
                return;
            }

            var securityRequirements = new Dictionary<string, IEnumerable<string>>
            {
                {"OAuth2", permissions}
            };

            if (operation.security == null)
            {
                operation.security = new List<IDictionary<string, IEnumerable<string>>>(1);
            }

            operation.security.Add(securityRequirements);
        }
    }
}
