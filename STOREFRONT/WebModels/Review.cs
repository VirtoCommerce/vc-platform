using DotLiquid;
using System;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Review : Drop
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public int ProductRating { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public DateTime? Created { get; set; }
    }
}