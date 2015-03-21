using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Fulfillment : Drop
    {
        [DataMember]
        public string TrackingCompany { get; set; }

        [DataMember]
        public string TrackingNumber { get; set; }

        [DataMember]
        public string TrackingUrl { get; set; }
    }
}