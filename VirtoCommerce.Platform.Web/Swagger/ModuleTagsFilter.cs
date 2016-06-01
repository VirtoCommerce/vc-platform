using System;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class ModuleTagsFilter : IOperationFilter
    {
        private readonly string _moduleId;

        public ModuleTagsFilter(string moduleId)
        {
            _moduleId = moduleId;
        }

        [CLSCompliant(false)]
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.tags = new[] { _moduleId };
        }
    }
}
