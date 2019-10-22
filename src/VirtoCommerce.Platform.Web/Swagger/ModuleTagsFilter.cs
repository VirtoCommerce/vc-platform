using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class ModuleTagsFilter : IOperationFilter
    {
        private readonly string _moduleId;

        public ModuleTagsFilter(string moduleId)
        {
            _moduleId = moduleId;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Tags = new List<OpenApiTag>
            {
                new OpenApiTag() { Name = _moduleId }
            };
        }
    }
}
