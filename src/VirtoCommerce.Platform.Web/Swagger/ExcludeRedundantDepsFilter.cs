using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VirtoCommerce.Platform.Web.Swagger
{
    /// <summary>
    /// Sometimes SwaggerIgnore can't be used for models, because no place to write SwaggerIgnore (the used model is a dependency).
    /// Here is a good place to exclude models from swagger document
    /// </summary>
    public class ExcludeRedundantDepsFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Components.Schemas.Remove(typeof(IdentityUserRole<string>).FullName);
        }
    }
}
