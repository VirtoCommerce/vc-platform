using DotLiquid;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Image : Drop
    {
        [DataMember]
        public string Alt { get; set; }

        [DataMember]
        public bool? AttachedToVariant { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public int Position { get; set; }

        [DataMember]
        public string Src { get; set; }

        [DataMember]
        public IEnumerable<Variant> Variants { get; set; }
    }
}