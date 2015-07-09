using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class TaxLine : Drop
    {
        [DataMember]
        public decimal RatePercentage
        {
            get
            {
                return Rate * 100;
            }
        }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}