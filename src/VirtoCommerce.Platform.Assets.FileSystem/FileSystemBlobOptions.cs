using System;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Assets.FileSystem
{
    [Obsolete("Deprecated. Use the same from FileSystemAssets module.")]
    public class FileSystemBlobOptions
    {
        /// <summary>
        /// The root folder where the files are stored
        /// </summary>
        [Required]
        public string RootPath { get; set; }
        /// <summary>
        /// Public base URL for direct access to files stored in the file system
        /// </summary>
        /// <example>
        /// http://localhost:8906/assets 
        /// </example>
        [Url]
        public string PublicUrl { get; set; }
    }
}
