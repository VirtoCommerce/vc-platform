using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Swagger
{
    public class FileResponseTypeFilter : IOperationFilter
    {
        [CLSCompliant(false)]
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {           
            if (IsFileResponse(apiDescription))
            {
                Schema responseSchema = new Schema { format = "byte", type = "file" };

                operation.responses[((int)HttpStatusCode.OK).ToString()] = new Response
                {
                    description = "OK",
                    schema = responseSchema
                };
            }
        }

        private static bool IsFileResponse(ApiDescription apiDescription)
        {
            var result = apiDescription.ActionDescriptor.GetCustomAttributes<SwaggerFileResponseAttribute>().Any();
            if (!result)
            {
                result = apiDescription.ResponseDescription.ResponseType == typeof(Stream);
            }
            return result;
        }
    }

}