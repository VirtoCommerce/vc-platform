using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class PaymentMethod : Drop
    {
        [DataMember]
        public string Handle { get; set; }
    }
}