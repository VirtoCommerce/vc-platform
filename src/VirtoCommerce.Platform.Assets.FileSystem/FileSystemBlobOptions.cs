using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Assets.FileSystem
{
    public class FileSystemBlobOptions
    {
        /// <summary>
        /// The root folder where the files are stored
        /// </summary>
        [Required]
        public string RootPath { get; set; }
        /// <summary>
        /// Public base URL for direct access to files stored in the file system
        /// Example:  http://localhost:8906/assets 
        /// </summary>
        [Url]
        public string PublicUrl { get; set; }
    }
}
