using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class PaymentMethod : Drop
    {
        [DataMember]
        public string Handle { get; set; }

        [DataMember]
        public string IconUrl { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}