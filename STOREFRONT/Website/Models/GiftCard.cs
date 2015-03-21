using DotLiquid;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class GiftCard : Drop
    {
        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public bool Expired { get; set; }

        [DataMember]
        public DateTime ExpiresOn { get; set; }

        [DataMember]
        public decimal InitialValue { get; set; }

        [DataMember]
        public Dictionary<string, string> Properties { get; set; }

        [DataMember]
        public string Url { get; set; }
    }
}