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

        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Tags = new[] { _moduleId };
        }
    }
}
