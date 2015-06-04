#region

using System.Runtime.Serialization;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models.Cms
{
    [DataContract]
    public class ThemeAsset : Drop
    {
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string ContentType { get; set; }

        [DataMember]
        public string Id { get; set; }
    }
}