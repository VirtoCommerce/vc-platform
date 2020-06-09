using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Web.SignalR
{
    public class AzureSignalRServiceOptions
    {
        [Required]
        public string ConnectionString { get; set; }
    }
}
