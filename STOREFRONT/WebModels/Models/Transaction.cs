using DotLiquid;
using System;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Transaction : Drop
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string StatusLabel { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public string Gateway { get; set; }
    }
}