using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class PaymentMethod : Drop
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string LogoUrl { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Group { get; set; }

        [DataMember]
        public bool IsAvailableForPartial { get; set; }

        [DataMember]
        public string CardNumber { get; set; }

        [DataMember]
        public string CardExpirationMonth { get; set; }

        [DataMember]
        public string CardExpirationYear { get; set; }

        [DataMember]
        public string CardCvv { get; set; }
    }
}