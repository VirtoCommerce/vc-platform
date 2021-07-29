using System;

namespace VirtoCommerce.Platform.Core.Swagger
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
        public string Description { get; set; } = "Upload File";
        /// <summary>
        /// Parameter type (only string value supported)
        /// Accordingly to: // https://swagger.io/docs/specification/describing-request-body/file-upload/
        /// </summary>
        public string Type { get; set; } = "string";
        /// <summary>
        /// Set true for required parameter
        /// </summary>
        public bool Required { get; set; } = false;
    }
}
