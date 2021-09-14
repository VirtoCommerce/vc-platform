using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage
{
    public class AzureBlobOptions
    {
        [Required]
        public string ConnectionString { get; set; }
        
        /// <summary>
        /// Url of the CDN server
        /// </summary>
        public string CdnUrl { get; set; }
    }
}
