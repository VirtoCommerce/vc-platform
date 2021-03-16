using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage
{
    public class AzureBlobOptions
    {
        [Required]
        public string ConnectionString { get; set; }
        public string CdnUrl { get; set; }
    }
}
