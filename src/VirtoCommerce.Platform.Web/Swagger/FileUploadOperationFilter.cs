using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VirtoCommerce.Platform.Web.Swagger
{
    //[CLSCompliant(false)]
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.TryGetMethodInfo(out var methodInfo))
            {
                var requestAttributes = methodInfo.GetCustomAttributes<UploadFileAttribute>().ToArray();
                operation.Parameters = operation.Parameters ?? new List<IParameter>();

                foreach (var attr in requestAttributes)
                {
                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = attr.Name,
                        Description = attr.Description,
                        In = "formData",
                        Required = attr.Required,
                        Type = attr.Type
                    });
                }

                if (requestAttributes.Any(x => x.Type == "file"))
                {
                    operation.Consumes.Add("multipart/form-data");
                }
            }
        }
    }
}
