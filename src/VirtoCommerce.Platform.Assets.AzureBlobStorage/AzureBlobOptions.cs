using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Storage.Blob;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage
{
    public class AzureBlobOptions
    {
        [Required]
        public string ConnectionString { get; set; }
        public string CdnUrl { get; set; }
        public BlobRequestOptions BlobRequestOptions { get; set; } = new BlobRequestOptions();
    }
}
