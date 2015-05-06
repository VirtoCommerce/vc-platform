using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class ShippingMethod : Drop
    {
        [DataMember]
        public string Handle { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}