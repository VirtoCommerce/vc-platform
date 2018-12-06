using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

using MvcAllowAnonymousAttribute = System.Web.Mvc.AllowAnonymousAttribute;
using WebApiAllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class AssignOAuth2SecurityOperationFilter : IOperationFilter
    {
        [CLSCompliant(false)]
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var actionFilters = apiDescription.ActionDescriptor.GetFilterPipeline();
            bool allowAnonymous = actionFilters.Select(x => x.Instance).OfType<OverrideAuthorizationAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }
            else
            {
                if (operation.security == null)
                {
                    operation.security = new List<IDictionary<string, IEnumerable<string>>>();
                }

                var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", new [] { OwinConfig.PublicClientId } }
                };

                operation.security.Add(oAuthRequirements);
            }
        }
    }
}
