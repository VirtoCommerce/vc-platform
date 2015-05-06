using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Discount : Drop
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public decimal Savings { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}