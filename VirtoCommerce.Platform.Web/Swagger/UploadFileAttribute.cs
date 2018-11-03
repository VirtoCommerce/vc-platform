using System;

namespace VirtoCommerce.Platform.Web.Swagger
{
    /// <summary> 
    /// Use this attribute for controllers methods to allow file upload via Swagger 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UploadFileAttribute : Attribute
    {
        /// <summary>
        /// The parameter name in the resulting swagger doc 
        /// </summary>
        public string Name { get; set; } = "uploadedFile";
        /// <summary>
        /// The parameter description in the resulting swagger doc 
        /// </summary>
        public string Description { get; set;  } = "Upload File";
        /// <summary>
        /// Parameter type (only File value supported)
        /// </summary>
        public string Type { get; set; } = "file";
        /// <summary>
        /// Set true for required parameter
        /// </summary>
        public bool Required { get; set; } = false; 
    }
}
