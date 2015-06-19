using DotLiquid;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models.Storage
{
    [DataContract]
    public class ProductDownloadLinks : Drop
    {
        public ProductDownloadLinks()
        {
            Links = new List<DownloadLink>();
        }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public ICollection<DownloadLink> Links { get; set; }
    }
}