using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Swagger
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UploadFileAttribute : Attribute
    {
        public UploadFileAttribute()
        {
            Name = "file";
            Description = "Upload file";
            Type = "file";
            Required = true;
        }

        public string Name { get; } 

        public string Description { get; } 
        public string Type { get; }

        public bool Required { get; } 
    }
}
