using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class TaxLine : Drop
    {
        [DataMember]
        public int Percentage { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public double Rate { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}